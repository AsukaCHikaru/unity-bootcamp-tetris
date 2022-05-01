using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tetrominoes {
    public class PositionMap {
        public int[,,] GetMap(BlockTypeList blockType) {
            if (blockType == BlockTypeList.I_block) {
                int[,,] i_map = new int[,,] {
                    { { 0, 1 }, { 0, 0 }, { 0, -1 }, { 0, -2 } },
                    { { -1, 0 }, { 0, 0 }, { 1, 0 }, { 2, 0 } },
                };
                return i_map;
            }

            if (blockType == BlockTypeList.J_block) {
                int[,,] j_map = new int[,,] {
                    { { 0, 1 }, { 0, 0 }, { 0, -1 }, { -1, -1 } },
                    { { 1, 0 }, { 0, 0 }, { -1, 0 }, { -1, 1 } },
                    { { 0, -1 }, { 0, 0 }, { 0, 1 }, { 1, 1 } },
                    { { -1, 0 }, { 0, 0 }, { 1, 0 }, { 1, -1 } },
                };
                return j_map;
            }

            if (blockType == BlockTypeList.L_block) {
                int[,,] l_map = new int[,,] {
                    { { 0, 1 }, { 0, 0 }, { 0, -1 }, { 1, -1 } },
                    { { 1, 0 }, { 0, 0 }, { -1, 0 }, { -1, -1 } },
                    { { 0, -1 }, { 0, 0 }, { 0, 1 }, { -1, 1 } },
                    { { -1, 0 }, { 0, 0 }, { 1, 0 }, { 1, 1 } },
                };
                return l_map;
            }

            if (blockType == BlockTypeList.square_block) {
                int[,,] square_map = new int[,,] {
                    { { 0, 0 }, { 0, 1 }, { 1, 1 }, { 1, 0 } },
                };
                return square_map;
            }

            if (blockType == BlockTypeList.T_block) {
                int[,,] t_map = new int[,,] {
                    { { -1, 0 }, { 0, 0 }, { 1, 0 }, { 0, -1 } },
                    { { 0, 1 }, { 0, 0 }, { 0, -1 }, { -1, 0 } },
                    { { 1, 0 }, { 0, 0 }, { -1, 0 }, { 0, 1 } },
                    { { 0, -1 }, { 0, 0 }, { 0, 1 }, { 1, 0 } },
                };
                return t_map;
            }

            if (blockType == BlockTypeList.Z_block) {
                int[,,] z_map = new int[,,] {
                    { { 0, -1 }, { 0, 0 }, { 1, 0 }, { 1, 1 } },
                    { { -1, 0 }, { 0, 0 }, { 0, -1 }, { 1, -1 } },
                };
                return z_map;
            }

            int[,,] s_map = new int[,,] {
                { { 0, -1 }, { 0, 0 }, { -1, 0 }, { -1, 1 } },
                { { -1, 0 }, { 0, 0 }, { 0, 1 }, { 1, 1 } },
            };
            return s_map;
        }
    }
}