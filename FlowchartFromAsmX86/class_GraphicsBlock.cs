using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace FlowchartFromAsmX86 {

    public partial class MainForm : Form {
        private class GraphicsBlock {
            public BlockModel model;
            public GraphicsBlockSizes sizes;
            public string[] rows; //all rows excluding number-row (first)

            public GraphicsBlockOwnSizes own_size = null; //For font fluctuation

            public GraphicsBlock(BlockModel model) {
                this.model = model;
            }
        }

        private class GraphicsBlockSizes {
            public ushort block_width, block_height;
            public ushort[] rows_locations;

            public ushort text_width, freeRows_count;

            public GraphicsBlockSizes(ushort block_width, ushort block_height, ushort freeRows_count, ushort text_width, ushort[] rows_locations) {
                this.block_width = block_width;
                this.block_height = block_height;
                this.rows_locations = rows_locations;

                this.freeRows_count = freeRows_count;
                this.text_width = text_width;
            }

            public GraphicsBlockSizes(GraphicsBlockSizes parent) {
                block_width = parent.block_width;
                block_height = parent.block_height;
                rows_locations = parent.rows_locations;

                freeRows_count = parent.freeRows_count;
                text_width = parent.text_width;
            }
        }

        private class GraphicsBlockOwnSizes {
            public ushort[] rows_locations;
            public ushort freeRows_count;
            public Font font;

            public GraphicsBlockOwnSizes(ushort freeRows_count, ushort[] rows_locations, Font font) {
                this.freeRows_count = freeRows_count;
                this.rows_locations = rows_locations;
                this.font = font;
            }
        }

        public class MaxHeap<T> : IDisposable where T : IComparable {
            private T[] _elements;
            private int _size;

            public MaxHeap(int size) {
                _elements = new T[size];
            }

            public void Dispose() {
                _elements = new T[0];
                _size = 0;
            }

            private int GetLeftChildIndex(int elementIndex) => 2 * elementIndex + 1;
            private int GetRightChildIndex(int elementIndex) => 2 * elementIndex + 2;
            private int GetParentIndex(int elementIndex) => (elementIndex - 1) / 2;

            private T GetLeftChild(int elementIndex) => _elements[GetLeftChildIndex(elementIndex)];
            private T GetRightChild(int elementIndex) => _elements[GetRightChildIndex(elementIndex)];
            private T GetParent(int elementIndex) => _elements[GetParentIndex(elementIndex)];

            private void Swap(int firstIndex, int secondIndex) {
                var temp = _elements[firstIndex];
                _elements[firstIndex] = _elements[secondIndex];
                _elements[secondIndex] = temp;
            }

            public T Pop() {
                var result = _elements[0];
                _elements[0] = _elements[_size - 1];
                --_size;

                ReCalculateDown();

                return result;
            }

            public void Add(T element) {
                _elements[_size] = element;
                ++_size;

                ReCalculateUp();
            }

            private void ReCalculateDown() {
                int index = 0;
                int biggerIndex;
                while ((biggerIndex = GetLeftChildIndex(index)) < _size) {
                    if (GetRightChildIndex(index) < _size && GetRightChild(index).CompareTo(GetLeftChild(index)) > 0) {
                        biggerIndex = GetRightChildIndex(index);
                    }

                    if (_elements[biggerIndex].CompareTo(_elements[index]) < 0) {
                        break;
                    }

                    Swap(biggerIndex, index);
                    index = biggerIndex;
                }
            }

            private void ReCalculateUp() {
                var index = _size - 1;
                while (index != 0 && _elements[index].CompareTo(GetParent(index)) > 0) {
                    var parentIndex = GetParentIndex(index);
                    Swap(parentIndex, index);
                    index = parentIndex;
                }
            }
        }

        private struct NumLinked : IComparable {
            public NumLinked(ushort num, int model_index) {
                this.num = num;
                this.model_index = model_index;
            }
            public ushort num;
            public int model_index;

            public int CompareTo(object obj) {
                NumLinked len = (NumLinked)obj;
                if (num == len.num) {
                    return 0;
                } else if (num > len.num) {
                    return 1;
                }
                return -1;
            }
        }

        private struct Peak {
            public Peak(ushort amplitude, int from, int to) {
                this.amplitude = amplitude;
                this.from = from;
                this.to = to;
            }
            public ushort amplitude;
            public int from, to;
        }

        private void GetGraphicsBlocks(List<BlockModel> dataModel, out List<GraphicsBlock> graph_blocks, out List<GraphicsBlockSizes> graph_blocks_sizes, Progress progress) {
            graph_blocks = new List<GraphicsBlock>();
            graph_blocks_sizes = new List<GraphicsBlockSizes>();

            Graphics gr = pic_flowchart.CreateGraphics();
            gr.PageUnit = GraphicsUnit.Pixel;

            ctx.Post(delegate { progress.clb_NewСhapter("Selection of sizes for blocks", 100); }, null);

            Dictionary<int, int> loop_start_to_end = new Dictionary<int, int>(); //Needs only for Russian

            List<NumLinked> bstr = new List<NumLinked>();
            for (int i = 0; i != dataModel.Count; ++i) {
                graph_blocks.Add(new GraphicsBlock(dataModel[i]));
                if (dataModel[i].figure != BlockModel.Figure.Connector) {
                    bstr.Add(new NumLinked((ushort)Math.Ceiling(gr.MeasureString(dataModel[i].row2.Replace("\n", ""), form_settings.font).Width), i));

                    if (form_settings.rb_langRussian.Checked && form_settings.cbx_loopSize.SelectedIndex == 1 && dataModel[i].figure == BlockModel.Figure.LoopS) {
                        string pstr = dataModel[i].row2.Split('\n')[0];
                        for (int j = i + 1; j < dataModel.Count; ++j) {
                            if (dataModel[j].figure == BlockModel.Figure.LoopE && dataModel[j].row2.Split('\n')[0] == pstr) {
                                loop_start_to_end.Add(i, j);
                                break;
                            }
                        }
                    }
                }
            }

            bstr = bstr.OrderBy(x => x.num).ToList(); //Convert to sorted list


            double lettersHeight = form_settings.font.SizeInPoints * form_settings.font.FontFamily.GetCellAscent(form_settings.font.Style) / form_settings.font.FontFamily.GetEmHeight(form_settings.font.Style),
                spaceHeight = lettersHeight * (double)form_settings.nmb_lineSpacing.Value,
                lineHeight = spaceHeight + lettersHeight,
                mini_lettersHeight, mini_spaceHeight, mini_lineHeight;
            ushort GraphicsBlockSizes_column_location = (ushort)Math.Round(spaceHeight / 2, MidpointRounding.AwayFromZero);

            {
                Font miniFont = new Font(form_settings.font.FontFamily, form_settings.font.Size - (float)form_settings.nmb_fontFlLimit.Value, form_settings.font.Style, GraphicsUnit.Point, 0);
                mini_lettersHeight = miniFont.SizeInPoints * miniFont.FontFamily.GetCellAscent(miniFont.Style) / miniFont.FontFamily.GetEmHeight(miniFont.Style);
                mini_spaceHeight = mini_lettersHeight * (double)form_settings.nmb_lineSpacing.Value;
                mini_lineHeight = mini_spaceHeight + mini_lettersHeight;
            }
            /* 
             * Equation to calculate minimal height of block:
             * (h-i-r)/l = c = L/(ah-i)
             * a = w/h of block; h = height of block; i = line spacing; r = line height; l = r + i; L = row length (in pixels); c = count of rows
             * a*(h^2) - h*(s+ai+al) + l*(i-L) + i^2 = 0 ;
             * Solution as a standard quadratic equation, h (height of block) is "x"
             */
            double a = (double)form_settings.nmb_ratioBlock.Value, // a
               b = -a * (spaceHeight + lettersHeight) - spaceHeight; // -a*(i+r) - i

            GraphicsBlock_TryEmplaceText_dlg GraphicsBlock_TryEmplaceText = (ref GraphicsBlock block, GraphicsBlockSizes size, out ushort min_increase_step, out List<string> brows, Graphics _, out bool need_width) => { return GraphicsBlock_TryEmplaceText_row(block.model.row2, block.model.figure, size, out min_increase_step, out brows, gr, out need_width, form_settings.font); };

            if (bstr.Count <= form_settings.nmb_countSizes.Value) { //Execution branch of "simple case" - this block contains a terminator
                ctx.Post(delegate { progress.clb_ChangeMaxAndCurrent(3, 1); }, null);

                if (form_settings.rb_langRussian.Checked && form_settings.cbx_loopSize.SelectedIndex == 2) {
                    double c = spaceHeight * (lettersHeight + spaceHeight) - bstr.First().num * lineHeight, // i*(r+i) - L*l
                       discriminant = b * b - 4 * a * c;
                    graph_blocks_sizes.Add(new GraphicsBlockSizes(0, 0, 0, 0, new ushort[] { GraphicsBlockSizes_column_location, (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight, MidpointRounding.AwayFromZero)) }));

                    foreach (var item in bstr) {
                        GraphicsBlock tmp = graph_blocks[item.model_index];

                        if (tmp.model.figure == BlockModel.Figure.LoopS || tmp.model.figure == BlockModel.Figure.LoopE) {
                            GraphicsBlockSizes sz = tmp.sizes = graph_blocks_sizes[0];
                            List<string> rows;

                            while (!GraphicsBlock_TryEmplaceText(ref tmp, sz, out ushort step, out rows, gr, out bool need_width)) {
                                sz.text_width += step;
                                sz.block_width = (ushort)(sz.text_width + Math.Round(spaceHeight, MidpointRounding.AwayFromZero));
                                sz.block_height = (ushort)Math.Round(sz.block_width / a, MidpointRounding.AwayFromZero);
                                if (need_width) {
                                    sz.freeRows_count = (ushort)((sz.block_height - spaceHeight - lettersHeight) / lineHeight);
                                } else {
                                    ushort temp = (ushort)((sz.block_height - spaceHeight - lettersHeight) / lineHeight);
                                    if (temp != sz.freeRows_count) {
                                        sz.freeRows_count = temp;
                                        sz.block_height = (ushort)Math.Round(temp * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                        sz.block_width = (ushort)Math.Round(sz.block_height * a, MidpointRounding.AwayFromZero);
                                        sz.text_width = (ushort)(sz.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                        { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                            ushort plen = (ushort)sz.rows_locations.Length, nlen = (ushort)(sz.freeRows_count + 1);
                                            Array.Resize(ref sz.rows_locations, nlen);
                                            for (ushort j = plen; j != nlen; ++j) {
                                                sz.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                            }
                                        }
                                    }
                                }
                            }
                            tmp.rows = rows.ToArray();
                        }
                    }
                }

                ctx.Post(delegate { progress.clb_ChangeCurrent(2); }, null);

                foreach (var item in bstr) {
                    GraphicsBlock tmp = graph_blocks[item.model_index];
                    if (tmp.sizes != null) continue;

                    double c = spaceHeight * (lettersHeight + spaceHeight) - item.num * lineHeight, // i*(r+i) - L*l
                        discriminant = b * b - 4 * a * c;

                    ushort block_height = (ushort)Math.Round((Math.Sqrt(discriminant) - b) / (2 * a), MidpointRounding.AwayFromZero),
                        block_width = (ushort)Math.Round(block_height * a, MidpointRounding.AwayFromZero),
                        rows_avail = (ushort)((block_height - spaceHeight - lettersHeight) / lineHeight),
                        wide_avail = (ushort)(block_width - spaceHeight);

                    graph_blocks_sizes.Add(new GraphicsBlockSizes(block_width, block_height, rows_avail, wide_avail, new ushort[] { GraphicsBlockSizes_column_location, (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight, MidpointRounding.AwayFromZero)) }));

                    GraphicsBlockSizes sz = graph_blocks_sizes.Last();

                    List<string> rows;
                    while (!GraphicsBlock_TryEmplaceText(ref tmp, sz, out ushort step, out rows, gr, out bool need_width)) {
                        sz.text_width += step;
                        sz.block_width = (ushort)(sz.text_width + Math.Round(spaceHeight, MidpointRounding.AwayFromZero));
                        sz.block_height = (ushort)Math.Round(sz.block_width / a, MidpointRounding.AwayFromZero);
                        if (need_width) {
                            sz.freeRows_count = (ushort)((sz.block_height - spaceHeight - lettersHeight) / lineHeight);
                        } else {
                            ushort temp = (ushort)((sz.block_height - spaceHeight - lettersHeight) / lineHeight);
                            if (temp != sz.freeRows_count) {
                                sz.freeRows_count = temp;
                                sz.block_height = (ushort)Math.Round(temp * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                sz.block_width = (ushort)Math.Round(sz.block_height * a, MidpointRounding.AwayFromZero);
                                sz.text_width = (ushort)(sz.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                    ushort plen = (ushort)sz.rows_locations.Length, nlen = (ushort)(sz.freeRows_count + 1);
                                    Array.Resize(ref sz.rows_locations, nlen);
                                    for (ushort j = plen; j != nlen; ++j) {
                                        sz.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                    }
                                }
                            }
                        }
                    }
                    tmp.sizes = sz;
                    tmp.rows = rows.ToArray();

                    if (form_settings.rb_langRussian.Checked && form_settings.cbx_loopSize.SelectedIndex == 1 && dataModel[item.model_index].figure == BlockModel.Figure.LoopS) { //Calculate sizes for pair LoopE
                        if (loop_start_to_end.TryGetValue(item.model_index, out int graph_blocks_idx)) {
                            tmp = graph_blocks[graph_blocks_idx];
                            while (!GraphicsBlock_TryEmplaceText(ref tmp, sz, out ushort step, out rows, gr, out bool need_width)) {
                                sz.text_width += step;
                                sz.block_width = (ushort)(sz.text_width + Math.Round(spaceHeight, MidpointRounding.AwayFromZero));
                                sz.block_height = (ushort)Math.Round(sz.block_width / a, MidpointRounding.AwayFromZero);
                                if (need_width) {
                                    sz.freeRows_count = (ushort)((sz.block_height - spaceHeight - lettersHeight) / lineHeight);
                                } else {
                                    ushort temp = (ushort)((sz.block_height - spaceHeight - lettersHeight) / lineHeight);
                                    if (temp != sz.freeRows_count) {
                                        sz.freeRows_count = temp;
                                        sz.block_height = (ushort)Math.Round(temp * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                        sz.block_width = (ushort)Math.Round(sz.block_height * a, MidpointRounding.AwayFromZero);
                                        sz.text_width = (ushort)(sz.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                        { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                            ushort plen = (ushort)sz.rows_locations.Length, nlen = (ushort)(sz.freeRows_count + 1);
                                            Array.Resize(ref sz.rows_locations, nlen);
                                            for (ushort j = plen; j != nlen; ++j) {
                                                sz.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                            }
                                        }
                                    }
                                }
                            }
                            tmp.sizes = sz;
                            tmp.rows = rows.ToArray();
                        }
                    }
                }

                ctx.Post(delegate { progress.Close(); }, null);
                return; //End of procedure
            }

            MaxHeap<NumLinked> heap = new MaxHeap<NumLinked>(bstr.Count);
            for (int i = 0; i != bstr.Count - 1; ++i) {
                heap.Add(new NumLinked((ushort)(bstr[i + 1].num - bstr[i].num), i));
            }

            List<NumLinked> lnsort = new List<NumLinked>((int)form_settings.nmb_countSizes.Value);
            float threshold = (float)(form_settings.nmb_strDiffThr.Value / 100);
            for (ushort i = 0; i != form_settings.nmb_countSizes.Value - 1; ++i) {
                NumLinked tmp = heap.Pop();
                if ((float)bstr[tmp.model_index].num / bstr[tmp.model_index + 1].num <= threshold)
                    lnsort.Add(tmp);
                else
                    break;
            }
            heap.Dispose();

            lnsort = lnsort.OrderBy(x => x.model_index).ToList(); //Convert to sorted by index list

            List<Peak> peaks = new List<Peak>(lnsort.Count + 1);
            int prevIndex = 0;
            foreach (var item in lnsort) {
                peaks.Add(new Peak(bstr[item.model_index].num, prevIndex, item.model_index));
                prevIndex = item.model_index + 1;
            }
            peaks.Add(new Peak(bstr.Last().num, prevIndex, bstr.Count - 1));

            lnsort.Clear();

            //Bitmap bmp = new Bitmap(256, 256, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //Graphics g0r = Graphics.FromImage(bmp);
            //g0r.Clear(Color.White);
            //g0r.DrawString("Rgj ЙЦуЩ", form_settings.font, Brushes.Black, 10, 10);
            //g0r.DrawString("Rgj ЙЦуЩ", form_settings.font, Brushes.Black, 10, 10 + (float)lineHeight);
            //pic_flowchart.Image = bmp;

            ctx.Post(delegate { progress.clb_ChangeMax(bstr.Count * 3); }, null);

            if (form_settings.chbx_allowFontFl.Checked) {
                GraphicsBlock_TryEmplaceText = GraphicsBlock_TryEmplaceText_ffl;
            }

            if (form_settings.rb_langEnglish.Checked || form_settings.cbx_loopSize.SelectedIndex == 0) {
                // Distributing sizes - Main part

                List<Point> distribute_later = new List<Point>();

                for (int cpk_idx = 0; cpk_idx != peaks.Count; ++cpk_idx) {
                    Peak cpk = peaks[cpk_idx];

                    {
                        double c = spaceHeight * (lettersHeight + spaceHeight) - cpk.amplitude * lineHeight, // i*(r+i) - L*l
                                                discriminant = b * b - 4 * a * c;
                        ushort block_height = (ushort)Math.Round((Math.Sqrt(discriminant) - b) / (2 * a), MidpointRounding.AwayFromZero),
                            block_width = (ushort)Math.Round(block_height * a, MidpointRounding.AwayFromZero),
                            rows_avail = (ushort)((block_height - spaceHeight - lettersHeight) / lineHeight),
                            wide_avail = (ushort)(block_width - spaceHeight);

                        if (rows_avail == 0) {
                            rows_avail = 1;
                            block_height = (ushort)Math.Round(lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                            block_width = (ushort)Math.Round(block_height * a, MidpointRounding.AwayFromZero);
                            wide_avail = (ushort)(block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));
                        }
                        graph_blocks_sizes.Add(new GraphicsBlockSizes(block_width, block_height, rows_avail, wide_avail, new ushort[] { GraphicsBlockSizes_column_location, (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight, MidpointRounding.AwayFromZero)) }));
                    }

                    GraphicsBlockSizes sz = graph_blocks_sizes.Last();

                    for (int i = cpk.to; i >= cpk.from; --i) {
                        GraphicsBlock tmp = graph_blocks[bstr[i].model_index];

                        if (tmp.model.row2.IndexOf('\n') == -1) {
                            List<string> rows;
                            if (!GraphicsBlock_TryEmplaceText(ref tmp, sz, out ushort step, out rows, gr, out bool need_width)) {
                                if (tmp.model.figure == BlockModel.Figure.Terminator || tmp.model.figure == BlockModel.Figure.FDecision || tmp.model.figure == BlockModel.Figure.JDecision) {
                                    distribute_later.Add(new Point(i, cpk_idx)); //Delayed distribution
                                    continue; //Skip resizing of sizeGroup because of different shape of the Terminator
                                }

                                ushort mini_freeRows_count = (ushort)((sz.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                                do { 
                                    sz.text_width += step;
                                    sz.block_width = (ushort)(sz.text_width + Math.Round(spaceHeight, MidpointRounding.AwayFromZero));
                                    sz.block_height = (ushort)Math.Round(sz.block_width / a, MidpointRounding.AwayFromZero);
                                    if (need_width) {
                                        sz.freeRows_count = (ushort)((sz.block_height - spaceHeight - lettersHeight) / lineHeight);
                                    } else {
                                        if (form_settings.chbx_allowFontFl.Checked) {
                                            ushort temp = (ushort)((sz.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                                            if (temp != mini_freeRows_count) {
                                                sz.block_height = (ushort)Math.Round(temp * mini_lineHeight + mini_spaceHeight + mini_lettersHeight, MidpointRounding.AwayFromZero);

                                                sz.freeRows_count = (ushort)((sz.block_height - spaceHeight - lettersHeight) / lineHeight);
                                                sz.block_height = (ushort)Math.Round(sz.freeRows_count * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                                sz.block_width = (ushort)Math.Round(sz.block_height * a, MidpointRounding.AwayFromZero);
                                                sz.text_width = (ushort)(sz.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));
                                                
                                                { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                                    ushort plen = (ushort)sz.rows_locations.Length, nlen = (ushort)(sz.freeRows_count + 1);
                                                    Array.Resize(ref sz.rows_locations, nlen);
                                                    for (ushort j = plen; j != nlen; ++j) {
                                                        sz.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                                    }
                                                }
                                            }
                                        } else {
                                            ushort temp = (ushort)((sz.block_height - spaceHeight - lettersHeight) / lineHeight);
                                            if (temp != sz.freeRows_count) {
                                                sz.freeRows_count = temp;
                                                sz.block_height = (ushort)Math.Round(temp * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                                sz.block_width = (ushort)Math.Round(sz.block_height * a, MidpointRounding.AwayFromZero);
                                                sz.text_width = (ushort)(sz.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                                { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                                    ushort plen = (ushort)sz.rows_locations.Length, nlen = (ushort)(sz.freeRows_count + 1);
                                                    Array.Resize(ref sz.rows_locations, nlen);
                                                    for (ushort j = plen; j != nlen; ++j) {
                                                        sz.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                                    }
                                                }
                                            }
                                        }
                                    }
                                } while (!GraphicsBlock_TryEmplaceText(ref tmp, sz, out step, out rows, gr, out need_width));
                            }
                            tmp.sizes = sz;
                            tmp.rows = rows.ToArray();
                        } else
                            distribute_later.Add(new Point(i, cpk_idx));

                        ctx.Post(delegate { progress.clb_Increment(); }, null);
                    }
                }

                ctx.Post(delegate { progress.clb_ChangePercent(33); }, null);

                // Distributing sizes - Second part: distribute delayed

                for (int cpi = distribute_later.Count - 1; distribute_later.Count != 0; --cpi) {
                    GraphicsBlock tod = graph_blocks[bstr[distribute_later[cpi].X].model_index], tmp;
                    List<string> rows;
                    for (int cpk = distribute_later[cpi].Y; cpk < peaks.Count - 1; ++cpk) {
                        tmp = graph_blocks[bstr[peaks[cpk].to].model_index];
                        if (GraphicsBlock_TryEmplaceText(ref tod, tmp.sizes, out ushort step, out rows, gr, out bool need_width)) {
                            tod.sizes = tmp.sizes;
                            tod.rows = rows.ToArray();
                            goto dsl_skip;
                        }
                    }
                    tmp = graph_blocks[bstr[peaks.Last().to].model_index];

                    ushort mini_freeRows_count = (ushort)((tmp.sizes.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                    while (!GraphicsBlock_TryEmplaceText(ref tod, tmp.sizes, out ushort step, out rows, gr, out bool need_width)) {
                        tmp.sizes.text_width += step;
                        tmp.sizes.block_width = (ushort)(tmp.sizes.text_width + Math.Round(spaceHeight, MidpointRounding.AwayFromZero));
                        tmp.sizes.block_height = (ushort)Math.Round(tmp.sizes.block_width / a, MidpointRounding.AwayFromZero);
                        if (need_width) {
                            tmp.sizes.freeRows_count = (ushort)((tmp.sizes.block_height - spaceHeight - lettersHeight) / lineHeight);
                        } else {
                            if (form_settings.chbx_allowFontFl.Checked) {
                                ushort temp = (ushort)((tmp.sizes.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                                if (temp != mini_freeRows_count) {
                                    tmp.sizes.block_height = (ushort)Math.Round(temp * mini_lineHeight + mini_spaceHeight + mini_lettersHeight, MidpointRounding.AwayFromZero);

                                    tmp.sizes.freeRows_count = (ushort)((tmp.sizes.block_height - spaceHeight - lettersHeight) / lineHeight);
                                    tmp.sizes.block_height = (ushort)Math.Round(tmp.sizes.freeRows_count * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                    tmp.sizes.block_width = (ushort)Math.Round(tmp.sizes.block_height * a, MidpointRounding.AwayFromZero);
                                    tmp.sizes.text_width = (ushort)(tmp.sizes.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                    { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                        ushort plen = (ushort)tmp.sizes.rows_locations.Length, nlen = (ushort)(tmp.sizes.freeRows_count + 1);
                                        Array.Resize(ref tmp.sizes.rows_locations, nlen);
                                        for (ushort j = plen; j != nlen; ++j) {
                                            tmp.sizes.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                        }
                                    }
                                }
                            } else {
                                ushort temp = (ushort)((tmp.sizes.block_height - spaceHeight - lettersHeight) / lineHeight);
                                if (temp != tmp.sizes.freeRows_count) {
                                    tmp.sizes.freeRows_count = temp;
                                    tmp.sizes.block_height = (ushort)Math.Round(temp * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                    tmp.sizes.block_width = (ushort)Math.Round(tmp.sizes.block_height * a, MidpointRounding.AwayFromZero);
                                    tmp.sizes.text_width = (ushort)(tmp.sizes.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                    { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                        ushort plen = (ushort)tmp.sizes.rows_locations.Length, nlen = (ushort)(tmp.sizes.freeRows_count + 1);
                                        Array.Resize(ref tmp.sizes.rows_locations, nlen);
                                        for (ushort j = plen; j != nlen; ++j) {
                                            tmp.sizes.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                        }
                                    }
                                }
                            }
                        }
                    }
                    tod.rows = rows.ToArray();
                    tod.sizes = tmp.sizes;
                dsl_skip:;
                    distribute_later.RemoveAt(cpi);

                    ctx.Post(delegate { progress.clb_Increment(); }, null);
                }

                ctx.Post(delegate { progress.clb_ChangePercent(66); }, null);

            } else if (form_settings.cbx_loopSize.SelectedIndex == 1) { //\/ Russian loop for one letter \/
                List<Point> distribute_later = new List<Point>();

                for (int cpk_idx = 0; cpk_idx != peaks.Count; ++cpk_idx) {
                    Peak cpk = peaks[cpk_idx];

                    {
                        double c = spaceHeight * (lettersHeight + spaceHeight) - cpk.amplitude * lineHeight, // i*(r+i) - L*l
                                                discriminant = b * b - 4 * a * c;
                        ushort block_height = (ushort)Math.Round((Math.Sqrt(discriminant) - b) / (2 * a), MidpointRounding.AwayFromZero),
                            block_width = (ushort)Math.Round(block_height * a, MidpointRounding.AwayFromZero),
                            rows_avail = (ushort)((block_height - spaceHeight - lettersHeight) / lineHeight),
                            wide_avail = (ushort)(block_width - spaceHeight);

                        if (rows_avail == 0) {
                            rows_avail = 1;
                            block_height = (ushort)Math.Round(lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                            block_width = (ushort)Math.Round(block_height * a, MidpointRounding.AwayFromZero);
                            wide_avail = (ushort)(block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));
                        }
                        graph_blocks_sizes.Add(new GraphicsBlockSizes(block_width, block_height, rows_avail, wide_avail, new ushort[] { GraphicsBlockSizes_column_location, (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight, MidpointRounding.AwayFromZero)) }));
                    }

                    GraphicsBlockSizes sz = graph_blocks_sizes.Last();

                    for (int i = cpk.to; i >= cpk.from; --i) { //Setting size of rest of blocks
                        GraphicsBlock tmp = graph_blocks[bstr[i].model_index];

                        if (tmp.sizes == null) {
                            if (tmp.model.row2.IndexOf('\n') == -1) {
                                List<string> rows;
                                if (!GraphicsBlock_TryEmplaceText(ref tmp, sz, out ushort step, out rows, gr, out bool need_width)) {
                                    if (tmp.model.figure == BlockModel.Figure.Terminator || tmp.model.figure == BlockModel.Figure.FDecision || tmp.model.figure == BlockModel.Figure.JDecision) {
                                        distribute_later.Add(new Point(i, cpk_idx)); //Delayed distribution
                                        continue; //Skip resizing of sizeGroup because of different shape of the Terminator
                                    }

                                    ushort mini_freeRows_count = (ushort)((sz.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                                    do {
                                        sz.text_width += step;
                                        sz.block_width = (ushort)(sz.text_width + Math.Round(spaceHeight, MidpointRounding.AwayFromZero));
                                        sz.block_height = (ushort)Math.Round(sz.block_width / a, MidpointRounding.AwayFromZero);
                                        if (need_width) {
                                            sz.freeRows_count = (ushort)((sz.block_height - spaceHeight - lettersHeight) / lineHeight);
                                        } else {
                                            if (form_settings.chbx_allowFontFl.Checked) {
                                                ushort temp = (ushort)((sz.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                                                if (temp != mini_freeRows_count) {
                                                    sz.block_height = (ushort)Math.Round(temp * mini_lineHeight + mini_spaceHeight + mini_lettersHeight, MidpointRounding.AwayFromZero);

                                                    sz.freeRows_count = (ushort)((sz.block_height - spaceHeight - lettersHeight) / lineHeight);
                                                    sz.block_height = (ushort)Math.Round(sz.freeRows_count * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                                    sz.block_width = (ushort)Math.Round(sz.block_height * a, MidpointRounding.AwayFromZero);
                                                    sz.text_width = (ushort)(sz.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                                    { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                                        ushort plen = (ushort)sz.rows_locations.Length, nlen = (ushort)(sz.freeRows_count + 1);
                                                        Array.Resize(ref sz.rows_locations, nlen);
                                                        for (ushort j = plen; j != nlen; ++j) {
                                                            sz.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                                        }
                                                    }
                                                }
                                            } else {
                                                ushort temp = (ushort)((sz.block_height - spaceHeight - lettersHeight) / lineHeight);
                                                if (temp != sz.freeRows_count) {
                                                    sz.freeRows_count = temp;
                                                    sz.block_height = (ushort)Math.Round(temp * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                                    sz.block_width = (ushort)Math.Round(sz.block_height * a, MidpointRounding.AwayFromZero);
                                                    sz.text_width = (ushort)(sz.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                                    { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                                        ushort plen = (ushort)sz.rows_locations.Length, nlen = (ushort)(sz.freeRows_count + 1);
                                                        Array.Resize(ref sz.rows_locations, nlen);
                                                        for (ushort j = plen; j != nlen; ++j) {
                                                            sz.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    } while (!GraphicsBlock_TryEmplaceText(ref tmp, sz, out step, out rows, gr, out need_width));
                                }
                                tmp.sizes = sz;
                                tmp.rows = rows.ToArray();

                                if (tmp.model.figure == BlockModel.Figure.LoopS) { //Calculate sizes for pair LoopE
                                    if (loop_start_to_end.TryGetValue(bstr[i].model_index, out int graph_blocks_idx)) {
                                        tmp = graph_blocks[graph_blocks_idx];

                                        ushort mini_freeRows_count = (ushort)((sz.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                                        while (!GraphicsBlock_TryEmplaceText(ref tmp, sz, out step, out rows, gr, out need_width)) {
                                            sz.text_width += step;
                                            sz.block_width = (ushort)(sz.text_width + Math.Round(spaceHeight, MidpointRounding.AwayFromZero));
                                            sz.block_height = (ushort)Math.Round(sz.block_width / a, MidpointRounding.AwayFromZero);
                                            if (need_width) {
                                                sz.freeRows_count = (ushort)((sz.block_height - spaceHeight - lettersHeight) / lineHeight);
                                            } else {
                                                if (form_settings.chbx_allowFontFl.Checked) {
                                                    ushort temp = (ushort)((sz.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                                                    if (temp != mini_freeRows_count) {
                                                        sz.block_height = (ushort)Math.Round(temp * mini_lineHeight + mini_spaceHeight + mini_lettersHeight, MidpointRounding.AwayFromZero);

                                                        sz.freeRows_count = (ushort)((sz.block_height - spaceHeight - lettersHeight) / lineHeight);
                                                        sz.block_height = (ushort)Math.Round(sz.freeRows_count * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                                        sz.block_width = (ushort)Math.Round(sz.block_height * a, MidpointRounding.AwayFromZero);
                                                        sz.text_width = (ushort)(sz.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                                        { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                                            ushort plen = (ushort)sz.rows_locations.Length, nlen = (ushort)(sz.freeRows_count + 1);
                                                            Array.Resize(ref sz.rows_locations, nlen);
                                                            for (ushort j = plen; j != nlen; ++j) {
                                                                sz.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                                            }
                                                        }
                                                    }
                                                } else {
                                                    ushort temp = (ushort)((sz.block_height - spaceHeight - lettersHeight) / lineHeight);
                                                    if (temp != sz.freeRows_count) {
                                                        sz.freeRows_count = temp;
                                                        sz.block_height = (ushort)Math.Round(temp * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                                        sz.block_width = (ushort)Math.Round(sz.block_height * a, MidpointRounding.AwayFromZero);
                                                        sz.text_width = (ushort)(sz.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                                        { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                                            ushort plen = (ushort)sz.rows_locations.Length, nlen = (ushort)(sz.freeRows_count + 1);
                                                            Array.Resize(ref sz.rows_locations, nlen);
                                                            for (ushort j = plen; j != nlen; ++j) {
                                                                sz.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        tmp.sizes = sz;
                                        tmp.rows = rows.ToArray();
                                    }
                                }
                            } else
                                distribute_later.Add(new Point(i, cpk_idx));
                        }

                        ctx.Post(delegate { progress.clb_Increment(); }, null);
                    }
                }

                ctx.Post(delegate { progress.clb_ChangePercent(33); }, null);

                // Distributing sizes - Second part: distribute delayed

                for (int cpi = distribute_later.Count - 1; distribute_later.Count != 0; --cpi) {
                    GraphicsBlock tod = graph_blocks[bstr[distribute_later[cpi].X].model_index];
                    if (tod.sizes == null) {
                        GraphicsBlock tmp;
                        List<string> rows;
                        for (int cpk = distribute_later[cpi].Y; cpk < peaks.Count - 1; ++cpk) {
                            tmp = graph_blocks[bstr[peaks[cpk].to].model_index];
                            if (GraphicsBlock_TryEmplaceText(ref tod, tmp.sizes, out ushort step, out rows, gr, out bool need_width)) {
                                tod.sizes = tmp.sizes;
                                tod.rows = rows.ToArray();
                                goto dsl_skip;
                            }
                        }
                        tmp = graph_blocks[bstr[peaks.Last().to].model_index];

                        ushort mini_freeRows_count = (ushort)((tmp.sizes.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                        while (!GraphicsBlock_TryEmplaceText(ref tod, tmp.sizes, out ushort step, out rows, gr, out bool need_width)) {
                            tmp.sizes.text_width += step;
                            tmp.sizes.block_width = (ushort)(tmp.sizes.text_width + Math.Round(spaceHeight, MidpointRounding.AwayFromZero));
                            tmp.sizes.block_height = (ushort)Math.Round(tmp.sizes.block_width / a, MidpointRounding.AwayFromZero);
                            if (need_width) {
                                tmp.sizes.freeRows_count = (ushort)((tmp.sizes.block_height - spaceHeight - lettersHeight) / lineHeight);
                            } else {
                                if (form_settings.chbx_allowFontFl.Checked) {
                                    ushort temp = (ushort)((tmp.sizes.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                                    if (temp != mini_freeRows_count) {
                                        tmp.sizes.block_height = (ushort)Math.Round(temp * mini_lineHeight + mini_spaceHeight + mini_lettersHeight, MidpointRounding.AwayFromZero);

                                        tmp.sizes.freeRows_count = (ushort)((tmp.sizes.block_height - spaceHeight - lettersHeight) / lineHeight);
                                        tmp.sizes.block_height = (ushort)Math.Round(tmp.sizes.freeRows_count * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                        tmp.sizes.block_width = (ushort)Math.Round(tmp.sizes.block_height * a, MidpointRounding.AwayFromZero);
                                        tmp.sizes.text_width = (ushort)(tmp.sizes.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                        { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                            ushort plen = (ushort)tmp.sizes.rows_locations.Length, nlen = (ushort)(tmp.sizes.freeRows_count + 1);
                                            Array.Resize(ref tmp.sizes.rows_locations, nlen);
                                            for (ushort j = plen; j != nlen; ++j) {
                                                tmp.sizes.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                            }
                                        }
                                    }
                                } else {
                                    ushort temp = (ushort)((tmp.sizes.block_height - spaceHeight - lettersHeight) / lineHeight);
                                    if (temp != tmp.sizes.freeRows_count) {
                                        tmp.sizes.freeRows_count = temp;
                                        tmp.sizes.block_height = (ushort)Math.Round(temp * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                        tmp.sizes.block_width = (ushort)Math.Round(tmp.sizes.block_height * a, MidpointRounding.AwayFromZero);
                                        tmp.sizes.text_width = (ushort)(tmp.sizes.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                        { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                            ushort plen = (ushort)tmp.sizes.rows_locations.Length, nlen = (ushort)(tmp.sizes.freeRows_count + 1);
                                            Array.Resize(ref tmp.sizes.rows_locations, nlen);
                                            for (ushort j = plen; j != nlen; ++j) {
                                                tmp.sizes.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        tod.rows = rows.ToArray();
                        tod.sizes = tmp.sizes;

                        if (tod.model.figure == BlockModel.Figure.LoopS) { //Calculate sizes for pair LoopE
                            tod = graph_blocks[loop_start_to_end[bstr[distribute_later[cpi].X].model_index]];

                            mini_freeRows_count = (ushort)((tmp.sizes.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                            while (!GraphicsBlock_TryEmplaceText(ref tod, tmp.sizes, out ushort step, out rows, gr, out bool need_width)) {
                                tmp.sizes.text_width += step;
                                tmp.sizes.block_width = (ushort)(tmp.sizes.text_width + Math.Round(spaceHeight, MidpointRounding.AwayFromZero));
                                tmp.sizes.block_height = (ushort)Math.Round(tmp.sizes.block_width / a, MidpointRounding.AwayFromZero);
                                if (need_width) {
                                    tmp.sizes.freeRows_count = (ushort)((tmp.sizes.block_height - spaceHeight - lettersHeight) / lineHeight);
                                } else {
                                    if (form_settings.chbx_allowFontFl.Checked) {
                                        ushort temp = (ushort)((tmp.sizes.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                                        if (temp != mini_freeRows_count) {
                                            tmp.sizes.block_height = (ushort)Math.Round(temp * mini_lineHeight + mini_spaceHeight + mini_lettersHeight, MidpointRounding.AwayFromZero);

                                            tmp.sizes.freeRows_count = (ushort)((tmp.sizes.block_height - spaceHeight - lettersHeight) / lineHeight);
                                            tmp.sizes.block_height = (ushort)Math.Round(tmp.sizes.freeRows_count * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                            tmp.sizes.block_width = (ushort)Math.Round(tmp.sizes.block_height * a, MidpointRounding.AwayFromZero);
                                            tmp.sizes.text_width = (ushort)(tmp.sizes.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                            { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                                ushort plen = (ushort)tmp.sizes.rows_locations.Length, nlen = (ushort)(tmp.sizes.freeRows_count + 1);
                                                Array.Resize(ref tmp.sizes.rows_locations, nlen);
                                                for (ushort j = plen; j != nlen; ++j) {
                                                    tmp.sizes.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                                }
                                            }
                                        }
                                    } else {
                                        ushort temp = (ushort)((tmp.sizes.block_height - spaceHeight - lettersHeight) / lineHeight);
                                        if (temp != tmp.sizes.freeRows_count) {
                                            tmp.sizes.freeRows_count = temp;
                                            tmp.sizes.block_height = (ushort)Math.Round(temp * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                            tmp.sizes.block_width = (ushort)Math.Round(tmp.sizes.block_height * a, MidpointRounding.AwayFromZero);
                                            tmp.sizes.text_width = (ushort)(tmp.sizes.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                            { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                                ushort plen = (ushort)tmp.sizes.rows_locations.Length, nlen = (ushort)(tmp.sizes.freeRows_count + 1);
                                                Array.Resize(ref tmp.sizes.rows_locations, nlen);
                                                for (ushort j = plen; j != nlen; ++j) {
                                                    tmp.sizes.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            tod.sizes = tmp.sizes;
                            tod.rows = rows.ToArray();
                        }
                    dsl_skip:;
                        distribute_later.RemoveAt(cpi);
                    }

                    ctx.Post(delegate { progress.clb_Increment(); }, null);
                }

                ctx.Post(delegate { progress.clb_ChangePercent(66); }, null);

            } else { //form_settings.cbx_loopSize.SelectedIndex == 2 // Russian loop for loop group
                List<Point> distribute_later = new List<Point>();
                GraphicsBlockSizes loopSize = null;
                GraphicsBlock tmp;

                for (int cpk_idx = peaks.Count - 1; cpk_idx != -1; --cpk_idx) {
                    Peak cpk = peaks[cpk_idx];

                    {
                        double c = spaceHeight * (lettersHeight + spaceHeight) - cpk.amplitude * lineHeight, // i*(r+i) - L*l
                                                discriminant = b * b - 4 * a * c;
                        ushort block_height = (ushort)Math.Round((Math.Sqrt(discriminant) - b) / (2 * a), MidpointRounding.AwayFromZero),
                            block_width = (ushort)Math.Round(block_height * a, MidpointRounding.AwayFromZero),
                            rows_avail = (ushort)((block_height - spaceHeight - lettersHeight) / lineHeight),
                            wide_avail = (ushort)(block_width - spaceHeight);

                        if (rows_avail == 0) {
                            rows_avail = 1;
                            block_height = (ushort)Math.Round(lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                            block_width = (ushort)Math.Round(block_height * a, MidpointRounding.AwayFromZero);
                            wide_avail = (ushort)(block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));
                        }
                        graph_blocks_sizes.Add(new GraphicsBlockSizes(block_width, block_height, rows_avail, wide_avail, new ushort[] { GraphicsBlockSizes_column_location, (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight, MidpointRounding.AwayFromZero)) }));
                    }

                    GraphicsBlockSizes sz = graph_blocks_sizes.Last();

                    for (int i = cpk.to; i >= cpk.from; --i) {
                        tmp = graph_blocks[bstr[i].model_index];

                        if (tmp.model.row2.IndexOf('\n') == -1) {
                            List<string> rows;

                            if (tmp.model.figure == BlockModel.Figure.LoopS || tmp.model.figure == BlockModel.Figure.LoopE) { // Special setter for loops
                                if (loopSize == null)
                                    loopSize = new GraphicsBlockSizes(sz);
                                ushort mini_freeRows_count = (ushort)((loopSize.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                                while (!GraphicsBlock_TryEmplaceText(ref tmp, loopSize, out ushort step, out rows, gr, out bool need_width)) {
                                    loopSize.text_width += step;
                                    loopSize.block_width = (ushort)(loopSize.text_width + Math.Round(spaceHeight, MidpointRounding.AwayFromZero));
                                    loopSize.block_height = (ushort)Math.Round(loopSize.block_width / a, MidpointRounding.AwayFromZero);
                                    if (need_width) {
                                        loopSize.freeRows_count = (ushort)((loopSize.block_height - spaceHeight - lettersHeight) / lineHeight);
                                    } else {
                                        if (form_settings.chbx_allowFontFl.Checked) {
                                            ushort temp = (ushort)((loopSize.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                                            if (temp != mini_freeRows_count) {
                                                loopSize.block_height = (ushort)Math.Round(temp * mini_lineHeight + mini_spaceHeight + mini_lettersHeight, MidpointRounding.AwayFromZero);

                                                loopSize.freeRows_count = (ushort)((loopSize.block_height - spaceHeight - lettersHeight) / lineHeight);
                                                loopSize.block_height = (ushort)Math.Round(loopSize.freeRows_count * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                                loopSize.block_width = (ushort)Math.Round(loopSize.block_height * a, MidpointRounding.AwayFromZero);
                                                loopSize.text_width = (ushort)(loopSize.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                                { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                                    ushort plen = (ushort)loopSize.rows_locations.Length, nlen = (ushort)(loopSize.freeRows_count + 1);
                                                    Array.Resize(ref loopSize.rows_locations, nlen);
                                                    for (ushort j = plen; j != nlen; ++j) {
                                                        loopSize.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                                    }
                                                }
                                            }
                                        } else {
                                            ushort temp = (ushort)((loopSize.block_height - spaceHeight - lettersHeight) / lineHeight);
                                            if (temp != loopSize.freeRows_count) {
                                                loopSize.freeRows_count = temp;
                                                loopSize.block_height = (ushort)Math.Round(temp * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                                loopSize.block_width = (ushort)Math.Round(loopSize.block_height * a, MidpointRounding.AwayFromZero);
                                                loopSize.text_width = (ushort)(loopSize.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                                { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                                    ushort plen = (ushort)loopSize.rows_locations.Length, nlen = (ushort)(loopSize.freeRows_count + 1);
                                                    Array.Resize(ref loopSize.rows_locations, nlen);
                                                    for (ushort j = plen; j != nlen; ++j) {
                                                        loopSize.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                tmp.sizes = loopSize;
                            } else {
                                if (!GraphicsBlock_TryEmplaceText(ref tmp, sz, out ushort step, out rows, gr, out bool need_width)) {
                                    if (tmp.model.figure == BlockModel.Figure.Terminator || tmp.model.figure == BlockModel.Figure.FDecision || tmp.model.figure == BlockModel.Figure.JDecision) {
                                        distribute_later.Add(new Point(i, cpk_idx)); //Delayed distribution
                                        continue; //Skip resizing of sizeGroup because of different shape of the Terminator
                                    }

                                    ushort mini_freeRows_count = (ushort)((sz.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                                    do {
                                        sz.text_width += step;
                                        sz.block_width = (ushort)(sz.text_width + Math.Round(spaceHeight, MidpointRounding.AwayFromZero));
                                        sz.block_height = (ushort)Math.Round(sz.block_width / a, MidpointRounding.AwayFromZero);
                                        if (need_width) {
                                            sz.freeRows_count = (ushort)((sz.block_height - spaceHeight - lettersHeight) / lineHeight);
                                        } else {
                                            if (form_settings.chbx_allowFontFl.Checked) {
                                                ushort temp = (ushort)((sz.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                                                if (temp != mini_freeRows_count) {
                                                    sz.block_height = (ushort)Math.Round(temp * mini_lineHeight + mini_spaceHeight + mini_lettersHeight, MidpointRounding.AwayFromZero);

                                                    sz.freeRows_count = (ushort)((sz.block_height - spaceHeight - lettersHeight) / lineHeight);
                                                    sz.block_height = (ushort)Math.Round(sz.freeRows_count * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                                    sz.block_width = (ushort)Math.Round(sz.block_height * a, MidpointRounding.AwayFromZero);
                                                    sz.text_width = (ushort)(sz.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                                    { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                                        ushort plen = (ushort)sz.rows_locations.Length, nlen = (ushort)(sz.freeRows_count + 1);
                                                        Array.Resize(ref sz.rows_locations, nlen);
                                                        for (ushort j = plen; j != nlen; ++j) {
                                                            sz.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                                        }
                                                    }
                                                }
                                            } else {
                                                ushort temp = (ushort)((sz.block_height - spaceHeight - lettersHeight) / lineHeight);
                                                if (temp != sz.freeRows_count) {
                                                    sz.freeRows_count = temp;
                                                    sz.block_height = (ushort)Math.Round(temp * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                                    sz.block_width = (ushort)Math.Round(sz.block_height * a, MidpointRounding.AwayFromZero);
                                                    sz.text_width = (ushort)(sz.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                                    { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                                        ushort plen = (ushort)sz.rows_locations.Length, nlen = (ushort)(sz.freeRows_count + 1);
                                                        Array.Resize(ref sz.rows_locations, nlen);
                                                        for (ushort j = plen; j != nlen; ++j) {
                                                            sz.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    } while (!GraphicsBlock_TryEmplaceText(ref tmp, sz, out step, out rows, gr, out need_width));
                                }
                                tmp.sizes = sz;
                            }
                            tmp.rows = rows.ToArray();
                        } else
                            distribute_later.Add(new Point(i, cpk_idx));

                        ctx.Post(delegate { progress.clb_Increment(); }, null);
                    }
                }

                ctx.Post(delegate { progress.clb_ChangePercent(33); }, null);

                // Distributing sizes - Second part: distribute delayed

                for (int cpi = distribute_later.Count - 1; distribute_later.Count != 0; --cpi) {
                    GraphicsBlock tod = graph_blocks[bstr[distribute_later[cpi].X].model_index];
                    List<string> rows;

                    if (tod.model.figure == BlockModel.Figure.LoopS || tod.model.figure == BlockModel.Figure.LoopE) {
                        if (loopSize == null)
                            loopSize = new GraphicsBlockSizes(0, 0, 0, 0, new ushort[] { GraphicsBlockSizes_column_location, (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight, MidpointRounding.AwayFromZero)) });
                        tod.sizes = loopSize;

                        ushort mini_freeRows_count = (ushort)((loopSize.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                        while (!GraphicsBlock_TryEmplaceText(ref tod, loopSize, out ushort step, out rows, gr, out bool need_width)) {
                            loopSize.text_width += step;
                            loopSize.block_width = (ushort)(loopSize.text_width + Math.Round(spaceHeight, MidpointRounding.AwayFromZero));
                            loopSize.block_height = (ushort)Math.Round(loopSize.block_width / a, MidpointRounding.AwayFromZero);
                            if (need_width) {
                                loopSize.freeRows_count = (ushort)((loopSize.block_height - spaceHeight - lettersHeight) / lineHeight);
                            } else {
                                if (form_settings.chbx_allowFontFl.Checked) {
                                    ushort temp = (ushort)((loopSize.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                                    if (temp != mini_freeRows_count) {
                                        loopSize.block_height = (ushort)Math.Round(temp * mini_lineHeight + mini_spaceHeight + mini_lettersHeight, MidpointRounding.AwayFromZero);

                                        loopSize.freeRows_count = (ushort)((loopSize.block_height - spaceHeight - lettersHeight) / lineHeight);
                                        loopSize.block_height = (ushort)Math.Round(loopSize.freeRows_count * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                        loopSize.block_width = (ushort)Math.Round(loopSize.block_height * a, MidpointRounding.AwayFromZero);
                                        loopSize.text_width = (ushort)(loopSize.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                        { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                            ushort plen = (ushort)loopSize.rows_locations.Length, nlen = (ushort)(loopSize.freeRows_count + 1);
                                            Array.Resize(ref loopSize.rows_locations, nlen);
                                            for (ushort j = plen; j != nlen; ++j) {
                                                loopSize.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                            }
                                        }
                                    }
                                } else {
                                    ushort temp = (ushort)((loopSize.block_height - spaceHeight - lettersHeight) / lineHeight);
                                    if (temp != loopSize.freeRows_count) {
                                        loopSize.freeRows_count = temp;
                                        loopSize.block_height = (ushort)Math.Round(temp * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                        loopSize.block_width = (ushort)Math.Round(loopSize.block_height * a, MidpointRounding.AwayFromZero);
                                        loopSize.text_width = (ushort)(loopSize.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                        { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                            ushort plen = (ushort)loopSize.rows_locations.Length, nlen = (ushort)(loopSize.freeRows_count + 1);
                                            Array.Resize(ref loopSize.rows_locations, nlen);
                                            for (ushort j = plen; j != nlen; ++j) {
                                                loopSize.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        tod.rows = rows.ToArray();
                    } else {
                        for (int cpk = distribute_later[cpi].Y; cpk < peaks.Count - 1; ++cpk) {
                            tmp = graph_blocks[bstr[peaks[cpk].to].model_index];

                            if (GraphicsBlock_TryEmplaceText(ref tod, tmp.sizes, out ushort step, out rows, gr, out bool need_width)) {
                                tod.sizes = tmp.sizes;
                                tod.rows = rows.ToArray();
                                goto dsl_skip;
                            }
                        }
                        tmp = graph_blocks[bstr[peaks.Last().to].model_index];

                        ushort mini_freeRows_count = (ushort)((tmp.sizes.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                        while (!GraphicsBlock_TryEmplaceText(ref tod, tmp.sizes, out ushort step, out rows, gr, out bool need_width)) {
                            tmp.sizes.text_width += step;
                            tmp.sizes.block_width = (ushort)(tmp.sizes.text_width + Math.Round(spaceHeight, MidpointRounding.AwayFromZero));
                            tmp.sizes.block_height = (ushort)Math.Round(tmp.sizes.block_width / a, MidpointRounding.AwayFromZero);
                            if (need_width) {
                                tmp.sizes.freeRows_count = (ushort)((tmp.sizes.block_height - spaceHeight - lettersHeight) / lineHeight);
                            } else {
                                if (form_settings.chbx_allowFontFl.Checked) {
                                    ushort temp = (ushort)((tmp.sizes.block_height - mini_spaceHeight - mini_lettersHeight) / mini_lineHeight);
                                    if (temp != mini_freeRows_count) {
                                        tmp.sizes.block_height = (ushort)Math.Round(temp * mini_lineHeight + mini_spaceHeight + mini_lettersHeight, MidpointRounding.AwayFromZero);

                                        tmp.sizes.freeRows_count = (ushort)((tmp.sizes.block_height - spaceHeight - lettersHeight) / lineHeight);
                                        tmp.sizes.block_height = (ushort)Math.Round(tmp.sizes.freeRows_count * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                        tmp.sizes.block_width = (ushort)Math.Round(tmp.sizes.block_height * a, MidpointRounding.AwayFromZero);
                                        tmp.sizes.text_width = (ushort)(tmp.sizes.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                        { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                            ushort plen = (ushort)tmp.sizes.rows_locations.Length, nlen = (ushort)(tmp.sizes.freeRows_count + 1);
                                            Array.Resize(ref tmp.sizes.rows_locations, nlen);
                                            for (ushort j = plen; j != nlen; ++j) {
                                                tmp.sizes.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                            }
                                        }
                                    }
                                } else {
                                    ushort temp = (ushort)((tmp.sizes.block_height - spaceHeight - lettersHeight) / lineHeight);
                                    if (temp != tmp.sizes.freeRows_count) {
                                        tmp.sizes.freeRows_count = temp;
                                        tmp.sizes.block_height = (ushort)Math.Round(temp * lineHeight + spaceHeight + lettersHeight, MidpointRounding.AwayFromZero);
                                        tmp.sizes.block_width = (ushort)Math.Round(tmp.sizes.block_height * a, MidpointRounding.AwayFromZero);
                                        tmp.sizes.text_width = (ushort)(tmp.sizes.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));

                                        { //Resize rows_locations to use it's values in GraphicsBlock_TryEmplaceText
                                            ushort plen = (ushort)tmp.sizes.rows_locations.Length, nlen = (ushort)(tmp.sizes.freeRows_count + 1);
                                            Array.Resize(ref tmp.sizes.rows_locations, nlen);
                                            for (ushort j = plen; j != nlen; ++j) {
                                                tmp.sizes.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        tod.sizes = tmp.sizes;
                        tod.rows = rows.ToArray();
                    }

                dsl_skip:;
                    distribute_later.RemoveAt(cpi);

                    ctx.Post(delegate { progress.clb_Increment(); }, null);
                }

                if (loopSize != null) { //Block for assigning one of the existing sizes to a group of cycles, or for correcting the largest size.
                    GraphicsBlockSizes sz = null;
                    for (int cpk_idx = 0; cpk_idx != peaks.Count; ++cpk_idx) {
                        sz = graph_blocks[bstr[peaks[cpk_idx].to].model_index].sizes;

                        if (sz.block_width >= loopSize.block_width) {
                            loopSize.block_width = sz.block_width;
                            loopSize.block_height = sz.block_height;
                            loopSize.text_width = sz.text_width;
                            loopSize.freeRows_count = sz.freeRows_count;
                            loopSize.rows_locations = sz.rows_locations;
                            goto loop_approx_succ;
                        }
                    }
                    sz.block_width = loopSize.block_width;
                    sz.block_height = loopSize.block_height;
                    sz.text_width = loopSize.text_width;
                    sz.freeRows_count = loopSize.freeRows_count;
                    sz.rows_locations = loopSize.rows_locations;
                loop_approx_succ:;
                }

                ctx.Post(delegate { progress.clb_ChangePercent(66); }, null);
            }

            //Correcting the row distribution for all blocks after changing the block size. It is not necessary to rearrange words in all blocks, but it is assumed that in many.
            {
                GraphicsBlock tmp;
                for (int i = 0; i != graph_blocks.Count; ++i) {
                    tmp = graph_blocks[i];
                    if (tmp.sizes != null) {
                        GraphicsBlock_TryEmplaceText(ref tmp, tmp.sizes, out ushort _, out List<string> rows, gr, out bool _);
                        tmp.rows = rows.ToArray();
                        GraphicsBlock_VerticalCenter(ref tmp, gr);

                        ctx.Post(delegate { progress.clb_Increment(); }, null);
                    }
                }
            }
            ctx.Post(delegate { progress.Close(); }, null);
        }

        private void GraphicsBlock_VerticalCenter(ref GraphicsBlock block, Graphics gr) {
            if (block.model.figure == BlockModel.Figure.Process || block.model.figure == BlockModel.Figure.Subprocess)
                return;

            
            if (block.own_size == null) {
                if (block.rows.Length < block.sizes.freeRows_count) {
                    List<Tuple<short, short>> metrics = new List<Tuple<short, short>>();

                    ushort appendments_before = 0, appendments_after = 0;
                    int hadd = (block.sizes.block_height - block.sizes.rows_locations[block.sizes.rows_locations.Length - 1] - block.sizes.rows_locations[1]); //Calculating the empty placeafter latest row end and border start
                    ushort pos_want = (ushort)Math.Round(block.sizes.block_height / 2F - ((block.rows.Length + 1 > block.sizes.rows_locations.Length ? block.sizes.rows_locations[block.rows.Length] + block.sizes.rows_locations[1] - block.sizes.rows_locations[0] : block.sizes.rows_locations[block.rows.Length + 1]) - block.sizes.rows_locations[0]) / 2F, MidpointRounding.AwayFromZero), pos_real;
                    
                    byte result = GraphicsBlock_ReEmplaceText(ref block, pos_want, (ushort)(hadd < 0 ? 0 : hadd), gr);

                    while (result != 0) {
                        if (result == 2) {
                            ++appendments_after;
                            block.model.row2 += "\n";
                        }

                        pos_want = (ushort)Math.Round(block.sizes.block_height / 2F - ((block.rows.Length + 1 > block.sizes.rows_locations.Length ? block.sizes.rows_locations[block.rows.Length] + block.sizes.rows_locations[1] - block.sizes.rows_locations[0] : block.sizes.rows_locations[block.rows.Length + 1]) - block.sizes.rows_locations[0]) / 2F, MidpointRounding.AwayFromZero);
                        result = GraphicsBlock_ReEmplaceText(ref block, pos_want, 0, gr);
                    }
                    pos_real = (ushort)Math.Round(block.sizes.block_height / 2F - ((block.rows.Length + 1 > block.sizes.rows_locations.Length ? block.sizes.rows_locations[block.rows.Length] + block.sizes.rows_locations[1] - block.sizes.rows_locations[0] : block.sizes.rows_locations[block.rows.Length + 1]) - block.sizes.rows_locations[0]) / 2F, MidpointRounding.AwayFromZero);
                    
                    metrics.Add(new Tuple<short, short>((short)(pos_real - pos_want), (short)(appendments_after - appendments_before)));
                }
            } else {
                if (block.rows.Length < block.own_size.freeRows_count) {
                    
                }
            }
        }


        delegate bool GraphicsBlock_TryEmplaceText_dlg(ref GraphicsBlock block, GraphicsBlockSizes size, out ushort min_increase_step, out List<string> brows, Graphics gr, out bool need_width);

        private void GraphicsBlock_TryEmplaceText_ffl_RecalcInside(ref GraphicsBlockSizes size, Font font) {
            double lettersHeight = font.SizeInPoints * font.FontFamily.GetCellAscent(font.Style) / font.FontFamily.GetEmHeight(font.Style),
                spaceHeight = lettersHeight * (double)form_settings.nmb_lineSpacing.Value,
                lineHeight = spaceHeight + lettersHeight;

            size.text_width = (ushort)(size.block_width - Math.Round(spaceHeight, MidpointRounding.AwayFromZero));
            size.freeRows_count = (ushort)((size.block_height - spaceHeight - lettersHeight) / lineHeight);
            
            ushort GraphicsBlockSizes_column_location = (ushort)Math.Round(spaceHeight / 2, MidpointRounding.AwayFromZero);
            Array.Resize(ref size.rows_locations, size.freeRows_count + 1);
            size.rows_locations[0] = GraphicsBlockSizes_column_location;
            size.rows_locations[1] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight, MidpointRounding.AwayFromZero));
            for (ushort j = 2; j != size.rows_locations.Length; ++j) {
                size.rows_locations[j] = (ushort)(GraphicsBlockSizes_column_location + Math.Round(lineHeight * j, MidpointRounding.AwayFromZero)); //The multiplication operation has not been replaced with an addition with the previous one to reduce the rounding error.
            }
        }

        private bool GraphicsBlock_TryEmplaceText_ffl(ref GraphicsBlock block, GraphicsBlockSizes size, out ushort min_increase_step, out List<string> brows, Graphics gr, out bool need_width) {
            if (GraphicsBlock_TryEmplaceText_row(block.model.row2, block.model.figure, size, out min_increase_step, out brows, gr, out need_width, form_settings.font)) {
                block.own_size = null;
                return true;
            } else {
                Font cfont = new Font(form_settings.font.FontFamily, form_settings.font.Size - (float)form_settings.nmb_fontFlLimit.Value, form_settings.font.Style, GraphicsUnit.Point, 0);
                GraphicsBlock_TryEmplaceText_ffl_RecalcInside(ref size, cfont);
                if (GraphicsBlock_TryEmplaceText_row(block.model.row2, block.model.figure, size, out min_increase_step, out brows, gr, out need_width, cfont)) {
                    float font_block_size = (float)form_settings.nmb_fontFlLimit.Value / 2, cfont_size = cfont.Size - (float)form_settings.nmb_fontFlLimit.Value;

                    while (font_block_size >= 0.01F) {
                        cfont = new Font(cfont.FontFamily, cfont_size + font_block_size, cfont.Style, GraphicsUnit.Point, 0);
                        GraphicsBlock_TryEmplaceText_ffl_RecalcInside(ref size, cfont);
                        if (GraphicsBlock_TryEmplaceText_row(block.model.row2, block.model.figure, size, out min_increase_step, out brows, gr, out need_width, cfont))
                            cfont_size += font_block_size;
                        font_block_size /= 2;
                    }

                    block.own_size = new GraphicsBlockOwnSizes(size.freeRows_count, size.rows_locations, cfont);
                    return true;
                } else {
                    return false;
                }
            }
        }

        /// <param name="rows_count">Count of rows including the index row</param>
        /// <returns>0 - success; 1 - Just fail; 2 - Empty row needed</returns>
        private byte GraphicsBlock_ReEmplaceText(ref GraphicsBlock block, ushort rows_offset, ushort rows_count, Graphics gr) {
            List<string> brows = new List<string>();
            ushort mis = 0;

            string[][] sliced = block.model.row2.Split('\n').Select(x => x.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToArray();

            ushort[] rows_locations;
            Font cfont;
            if (block.own_size == null) {
                rows_locations = block.sizes.rows_locations;
                cfont = form_settings.font;
            } else {
                rows_locations = block.own_size.rows_locations;
                cfont = block.own_size.font;
            }
            ushort cpr = 1; ++rows_count;
            switch (block.model.figure) {
                case BlockModel.Figure.Hexagon: {
                    ushort max_add = (ushort)(block.sizes.block_width - block.sizes.block_height), row_line_height = (ushort)(rows_locations[1] - rows_locations[0]), border = (ushort)(rows_locations[0] << 1);
                    float height_half = block.sizes.block_height / 2F, k = max_add / height_half;
                    for (ushort i = 0; i < sliced.Length; ++i) {
                        string[] source = sliced[i];
                        for (; cpr != rows_count; ++cpr) {
                            if (GraphicsBlock_TryEmplaceText_words(source, ref brows, out string[] remains, (ushort)(block.sizes.block_height + Math.Round(max_add - k * Math.Max(Math.Abs(rows_offset + rows_locations[cpr] - height_half), Math.Abs(rows_offset + rows_locations[cpr] + row_line_height - height_half)) - border, MidpointRounding.AwayFromZero)), ref mis, gr, cfont)) {
                                source = remains;
                            } else {
                                return 2; //Empty row append needed (\n) OR humility because of block have a border (as some capacity). That's a REALLY RARE CASE!
                            }
                        }
                        if (source.Length != 0) {
                            return 1; //Just fail
                        }
                    }
                }
                break;

                case BlockModel.Figure.IO: {
                    ushort row_line_height = (ushort)(rows_locations[1] - rows_locations[0]), width_half = (ushort)(block.sizes.block_width >> 1), border = (ushort)(rows_locations[0] << 1);
                    float max_add = block.sizes.block_height / 4F, height_half = block.sizes.block_height / 2F;
                    for (ushort i = 0; i < sliced.Length; ++i) {
                        string[] source = sliced[i];
                        for (; cpr != rows_count; ++cpr) {
                            if (GraphicsBlock_TryEmplaceText_words(source, ref brows, out string[] remains, (ushort)(width_half + Math.Round(max_add - 0.5F * Math.Max(Math.Abs(rows_offset + rows_locations[cpr] - height_half), Math.Abs(rows_offset + rows_locations[cpr] + row_line_height - height_half)), MidpointRounding.AwayFromZero) - border), ref mis, gr, cfont)) {
                                source = remains;
                            } else {
                                return 2; //Empty row append needed (\n) OR humility because of block have a border (as some capacity). That's a REALLY RARE CASE!
                            }
                        }
                        if (source.Length != 0) {
                            return 1; //Just fail
                        }
                    }
                }
                break;

                case BlockModel.Figure.LoopS: {
                    float max_sub = block.sizes.block_height / 4F;
                    for (ushort i = 0; i < sliced.Length; ++i) {
                        string[] source = sliced[i];
                        for (; cpr != rows_count; ++cpr) {
                            if (GraphicsBlock_TryEmplaceText_words(source, ref brows, out string[] remains, (ushort)Math.Round(rows_offset + rows_locations[cpr] > max_sub ? block.sizes.text_width : block.sizes.text_width - (max_sub + rows_offset + rows_locations[cpr]) * 2, MidpointRounding.AwayFromZero), ref mis, gr, cfont)) {
                                source = remains;
                            } else {
                                return 2; //Empty row append needed (\n) OR humility because of block have a border (as some capacity). That's a REALLY RARE CASE!
                            }
                        }
                        if (source.Length != 0) {
                            return 1; //Just fail
                        }
                    }
                }
                break;

                case BlockModel.Figure.LoopE: {
                    ushort row_line_height = (ushort)(rows_locations[1] - rows_locations[0]);
                    float max_sub = block.sizes.block_height / 4F;
                    for (ushort i = 0; i < sliced.Length; ++i) {
                        string[] source = sliced[i];
                        for (; cpr != rows_count; ++cpr) {
                            if (GraphicsBlock_TryEmplaceText_words(source, ref brows, out string[] remains, (ushort)Math.Round(block.sizes.block_height - rows_offset - rows_locations[cpr] - row_line_height < max_sub ? block.sizes.text_width : block.sizes.text_width - max_sub * 2 + (block.sizes.block_height - rows_offset - rows_locations[cpr] - row_line_height) * 2, MidpointRounding.AwayFromZero), ref mis, gr, cfont)) {
                                source = remains;
                            } else {
                                return 2; //Empty row append needed (\n) OR humility because of block have a border (as some capacity). That's a REALLY RARE CASE!
                            }
                        }
                        if (source.Length != 0) {
                            return 1; //Just fail
                        }
                    }
                }
                break;

                case BlockModel.Figure.Terminator: {
                    short free_rows = (short)(((block.sizes.block_height / 2.0) - (rows_locations[0] << 1)) / (rows_locations[1] - rows_locations[0]));
                    if (free_rows == 1 && block.rows.Length == free_rows)
                        return 0; //Only 1 row or everything is fitted. Nothing to do.

                    ushort row_line_height = (ushort)(rows_locations[1] - rows_locations[0]);
                    float R = block.sizes.block_height / 4.0F, Rsq = R * R;
                    for (ushort i = 0; i < sliced.Length; ++i) {
                        string[] source = sliced[i];
                        for (cpr = 0; cpr != free_rows; ++cpr) {
                            if (GraphicsBlock_TryEmplaceText_words(source, ref brows, out string[] remains, (ushort)(block.sizes.block_width - Math.Round((R - Math.Sqrt(Rsq - Math.Max(Square(rows_offset + rows_locations[cpr] - R), Square(rows_offset + rows_locations[cpr] + row_line_height - R))) - rows_locations[0]) * 2, MidpointRounding.AwayFromZero)), ref mis, gr, cfont)) {
                                source = remains;
                            } else {
                                return 2; //Empty row append needed (\n) OR humility because of block have a border (as some capacity). That's a REALLY RARE CASE!
                            }
                        }
                        if (source.Length != 0) {
                            return 1; //Just fail
                        }
                    }
                    /* Keep in mind:
                     size.freeRows_count = (ushort)free_rows;
                     size.block_height >>= 1;*/
                }
                break;

                case BlockModel.Figure.FDecision:
                case BlockModel.Figure.JDecision: {
                    ushort row_line_height = (ushort)(rows_locations[1] - rows_locations[0]), border = (ushort)(rows_locations[0] << 1);
                    float height_half = block.sizes.block_height / 2F, k = (block.sizes.block_width << 1) / (float)block.sizes.block_height;
                    for (ushort i = 0; i < sliced.Length; ++i) {
                        string[] source = sliced[i];
                        for (; cpr != rows_count; ++cpr) {
                            if (GraphicsBlock_TryEmplaceText_words(source, ref brows, out string[] remains, (ushort)Math.Round(k * (height_half - Math.Max(Math.Abs(rows_offset + rows_locations[cpr] - height_half), Math.Abs(rows_offset + rows_locations[cpr] + row_line_height - height_half))) - border, MidpointRounding.AwayFromZero), ref mis, gr, cfont)) {
                                source = remains;
                            } else {
                                return 2; //Empty row append needed (\n) OR humility because of block have a border (as some capacity). That's a REALLY RARE CASE!
                            }
                        }
                        if (source.Length != 0) {
                            return 1; //Just fail
                        }
                    }
                }
                break;
            }

            block.rows = brows.ToArray();
            return 0; //All Right!
        }

        private bool GraphicsBlock_TryEmplaceText_row(string row, BlockModel.Figure figure, GraphicsBlockSizes size, out ushort min_increase_step, out List<string> brows, Graphics gr, out bool need_width, Font cfont) {
            brows = new List<string>();
            min_increase_step = ushort.MaxValue;

            string[][] sliced = row.Split('\n').Select(x => x.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToArray();

            ushort cpr = 1, rows_count = (ushort)(size.freeRows_count + 1);
            switch (figure) {
                case BlockModel.Figure.Process:
                    for (ushort i = 0; i < sliced.Length; ++i) {
                        string[] source = sliced[i];
                        for (; cpr != rows_count; ++cpr) {
                            if (GraphicsBlock_TryEmplaceText_words(source, ref brows, out string[] remains, size.text_width, ref min_increase_step, gr, cfont)) {
                                source = remains;
                            } else {
                                need_width = true;
                                return false;
                            }
                        }
                        if (source.Length != 0) {
                            need_width = false;
                            return false;
                        }
                    }
                    break;
                case BlockModel.Figure.Subprocess: {
                    ushort text_size = (ushort)(size.text_width - Math.Round(0.3 * size.block_height, MidpointRounding.AwayFromZero));
                    for (ushort i = 0; i < sliced.Length; ++i) {
                        string[] source = sliced[i];
                        for (; cpr != rows_count; ++cpr) {
                            if (GraphicsBlock_TryEmplaceText_words(source, ref brows, out string[] remains, text_size, ref min_increase_step, gr, cfont)) {
                                source = remains;
                            } else {
                                need_width = true;
                                return false;
                            }
                        }
                        if (source.Length != 0) {
                            need_width = false;
                            return false;
                        }
                    }
                }
                break;

                case BlockModel.Figure.Hexagon: {
                    ushort max_add = (ushort)(size.block_width - size.block_height), row_line_height = (ushort)(size.rows_locations[1] - size.rows_locations[0]), border = (ushort)(size.rows_locations[0] << 1);
                    float height_half = size.block_height / 2F, k = max_add / height_half;
                    for (ushort i = 0; i < sliced.Length; ++i) {
                        string[] source = sliced[i];
                        for (; cpr != rows_count; ++cpr) {
                            if (GraphicsBlock_TryEmplaceText_words(source, ref brows, out string[] remains, (ushort)(size.block_height + Math.Round(max_add - k * Math.Max(Math.Abs(size.rows_locations[cpr] - height_half), Math.Abs(size.rows_locations[cpr] + row_line_height - height_half)) - border, MidpointRounding.AwayFromZero)), ref min_increase_step, gr, cfont)) {
                                source = remains;
                            } else if (size.rows_locations[cpr] + row_line_height > height_half) {
                                need_width = true;
                                return false;
                            }
                        }
                        if (source.Length != 0) {
                            need_width = false;
                            return false;
                        }
                    }
                }
                break;

                case BlockModel.Figure.IO: {
                    ushort row_line_height = (ushort)(size.rows_locations[1] - size.rows_locations[0]), width_half = (ushort)(size.block_width >> 1), border = (ushort)(size.rows_locations[0] << 1);
                    float max_add = size.block_height / 4F, height_half = size.block_height / 2F;
                    for (ushort i = 0; i < sliced.Length; ++i) {
                        string[] source = sliced[i];
                        for (; cpr != rows_count; ++cpr) {
                            if (GraphicsBlock_TryEmplaceText_words(source, ref brows, out string[] remains, (ushort)(width_half + Math.Round(max_add - 0.5F * Math.Max(Math.Abs(size.rows_locations[cpr] - height_half), Math.Abs(size.rows_locations[cpr] + row_line_height - height_half)), MidpointRounding.AwayFromZero) - border), ref min_increase_step, gr, cfont)) {
                                source = remains;
                            } else if (size.rows_locations[cpr] + row_line_height > height_half) {
                                need_width = true;
                                return false;
                            }
                        }
                        if (source.Length != 0) {
                            need_width = false;
                            return false;
                        }
                    }
                }
                break;

                case BlockModel.Figure.LoopS: {
                    float max_sub = size.block_height / 4F;
                    for (ushort i = 0; i < sliced.Length; ++i) {
                        string[] source = sliced[i];
                        for (; cpr != rows_count; ++cpr) {
                            if (GraphicsBlock_TryEmplaceText_words(source, ref brows, out string[] remains, (ushort)Math.Round(size.rows_locations[cpr] > max_sub ? size.text_width : size.text_width - (max_sub + size.rows_locations[cpr]) * 2, MidpointRounding.AwayFromZero), ref min_increase_step, gr, cfont)) {
                                source = remains;
                            } else if (size.rows_locations[cpr] + size.rows_locations[1] - size.rows_locations[0] > max_sub) {
                                need_width = true;
                                return false;
                            }
                        }
                        if (source.Length != 0) {
                            need_width = false;
                            return false;
                        }
                    }
                }
                break;

                case BlockModel.Figure.LoopE: {
                    ushort row_line_height = (ushort)(size.rows_locations[1] - size.rows_locations[0]);
                    float max_sub = size.block_height / 4F;
                    for (ushort i = 0; i < sliced.Length; ++i) {
                        string[] source = sliced[i];
                        for (; cpr != rows_count; ++cpr) {
                            if (GraphicsBlock_TryEmplaceText_words(source, ref brows, out string[] remains, (ushort)Math.Round(size.block_height - size.rows_locations[cpr] - row_line_height < max_sub ? size.text_width : size.text_width - max_sub * 2 + (size.block_height - size.rows_locations[cpr] - row_line_height) * 2, MidpointRounding.AwayFromZero), ref min_increase_step, gr, cfont)) {
                                source = remains;
                            } else {
                                need_width = true;
                                return false;
                            }
                        }
                        if (source.Length != 0) {
                            need_width = false;
                            return false;
                        }
                    }
                }
                break;

                case BlockModel.Figure.Terminator: {
                    short free_rows = (short)(((size.block_height / 2.0) - (size.rows_locations[0] << 1)) / (size.rows_locations[1] - size.rows_locations[0]));
                    if (free_rows == 0) {
                        min_increase_step = (ushort)(Math.Round(((size.rows_locations[1] - size.rows_locations[0] + (size.rows_locations[0] << 1)) << 1) * (double)form_settings.nmb_ratioBlock.Value, MidpointRounding.AwayFromZero) - size.block_width); //Calculating inctrement to reach 1 row in the Terminator shape 
                        need_width = false;
                        return false;
                    }
                    ushort row_line_height = (ushort)(size.rows_locations[1] - size.rows_locations[0]);
                    float R = size.block_height / 4.0F, Rsq = R * R;
                    for (ushort i = 0; i < sliced.Length; ++i) {
                        string[] source = sliced[i];
                        for (cpr = 0; cpr != free_rows; ++cpr) {
                            if (GraphicsBlock_TryEmplaceText_words(source, ref brows, out string[] remains, (ushort)(size.block_width - Math.Round((R - Math.Sqrt(Rsq - Math.Max(Square(size.rows_locations[cpr] - R), Square(size.rows_locations[cpr] + row_line_height - R))) - size.rows_locations[0]) * 2, MidpointRounding.AwayFromZero)), ref min_increase_step, gr, cfont)) {
                                source = remains;
                            } else if (size.rows_locations[cpr] + row_line_height > R) {
                                need_width = true;
                                return false;
                            }
                        }
                        if (source.Length != 0) {
                            need_width = false;
                            return false;
                        }
                    }
                    /* Keep in mind:
                     size.freeRows_count = (ushort)free_rows;
                     size.block_height >>= 1;*/
                }
                break;

                case BlockModel.Figure.FDecision:
                case BlockModel.Figure.JDecision: {
                    ushort row_line_height = (ushort)(size.rows_locations[1] - size.rows_locations[0]), border = (ushort)(size.rows_locations[0] << 1);
                    float height_half = size.block_height / 2F, k = (size.block_width << 1) / (float)size.block_height;
                    for (ushort i = 0; i < sliced.Length; ++i) {
                        string[] source = sliced[i];
                        for (; cpr != rows_count; ++cpr) {
                            if (GraphicsBlock_TryEmplaceText_words(source, ref brows, out string[] remains, (ushort)Math.Round(k * (height_half - Math.Max(Math.Abs(size.rows_locations[cpr] - height_half), Math.Abs(size.rows_locations[cpr] + row_line_height - height_half))) - border, MidpointRounding.AwayFromZero), ref min_increase_step, gr, cfont)) {
                                source = remains;
                            } else if (size.rows_locations[cpr] + row_line_height > height_half) {
                                need_width = true;
                                return false;
                            }
                        }
                        if (source.Length != 0) {
                            need_width = false;
                            return false;
                        }
                    }
                }
                break;
            }

            need_width = false;
            return true;
        }

        private bool GraphicsBlock_TryEmplaceText_words(string[] words, ref List<string> brows, out string[] remains, ushort text_width, ref ushort min_increase_step, Graphics gr, Font cfont) {
            ushort join_count = (ushort)words.Length, join_count_prew, join_count_block;
            ushort csize = (ushort)Math.Ceiling(gr.MeasureString(string.Join(" ", words.Take(join_count)), cfont).Width);
            remains = null;
            while (csize > text_width) {
                join_count >>= 1;
                if (join_count == 0) {
                    min_increase_step = (ushort)(text_width - csize);
                    return false;
                }
                csize = (ushort)Math.Ceiling(gr.MeasureString(string.Join(" ", words.Take(join_count)), cfont).Width);
            }

            join_count_block = join_count_prew = join_count;
            if (csize < min_increase_step) min_increase_step = csize;
            while ((join_count_block >>= 1) != 0) {
                join_count = (ushort)(join_count_prew + join_count_block);
                csize = (ushort)Math.Ceiling(gr.MeasureString(string.Join(" ", words.Take(join_count)), cfont).Width);
                if (csize <= text_width)
                    join_count_prew = join_count;
                else if (csize < min_increase_step)
                    min_increase_step = csize;
            }
            join_count = join_count_prew;
            do {
                ++join_count;
                csize = (ushort)Math.Ceiling(gr.MeasureString(string.Join(" ", words.Take(join_count)), cfont).Width);
            } while (join_count < words.Length && csize <= text_width);

            brows.Add(string.Join(" ", words.Take(--join_count)));
            remains = words.Skip(join_count).ToArray();
            return true;
        }

        private double Square(double x) {
            return x * x;
        }

    }
}
