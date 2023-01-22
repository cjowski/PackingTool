import type { IPackingItem } from "../item/IPackingItem"
import type { PackingItemType } from "../item/PackingItemType"

export interface IPackingGroup {
  readonly ID: number
  Name: string
  Type: PackingItemType
  Sort: number
  Items: IPackingItem[]

  Synchronize(otherGroup: IPackingGroup): void
  AddItem(id: number, name: string, count: number): void
  AddItems(newItems: IPackingItem[]): void
  RemoveItem(itemID: number): void
}
