import type { PackingItemAttribute } from "./PackingItemAttribute"

export interface IPackingItem {
  readonly ID: number
  Name: string
  Count: number
  Sort: number
  Attributes: PackingItemAttribute[],
  Packed: boolean
}
