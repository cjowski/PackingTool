import type { IPackingGroup } from "../group/IPackingGroup"

export interface IPackingList {
  GridColumnCount: number
  Groups: IPackingGroup[]
}
