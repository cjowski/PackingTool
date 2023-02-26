import type { PackingList as ApiPackingList } from "src/api/models/PackingList"
import { PackingListContent } from "./PackingListContent"
import { PackingListState } from "./PackingListState"

export class PackingList implements ApiPackingList {
  id: number
  name: string
  content: PackingListContent
  state: PackingListState

  constructor(
    id: number,
    name: string,
    content: PackingListContent,
    state: PackingListState
  ) {
    this.id = id
    this.name = name
    this.content = content
    this.state = state
  }

  static New(name: string) {
    return new PackingList(
      0,
      name,
      PackingListContent.New(),
      PackingListState.New
    )
  }

  static Unsynced(id: number, name: string) {
    return new PackingList(
      id,
      name,
      PackingListContent.New(),
      PackingListState.Unsynced
    )
  }

  static Undefined() {
    return new PackingList(
      0,
      "",
      PackingListContent.New(),
      PackingListState.Undefined
    )
  }

  ToJson() {
    return <ApiPackingList>{
      id: this.id,
      name: this.name,
      content: this.content.ToJson(),
    }
  }

  Synchronize(otherList: ApiPackingList, otherListName: string) {
    this.name = otherListName
    this.content.Synchronize(otherList.content)
    this.state = PackingListState.Synced
  }
}
