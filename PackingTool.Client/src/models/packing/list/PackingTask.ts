import type { PackingTask as ApiPackingTask } from "src/api/models/PackingTask"

export class PackingTask implements ApiPackingTask {
  id: number
  name: string
  done: boolean

  private constructor(id: number, name: string, done: boolean) {
    this.id = id
    this.name = name
    this.done = done
  }

  static New(id: number, name: string) {
    return new PackingTask(id, name, false)
  }

  static FromJson(json: ApiPackingTask) {
    return new PackingTask(
      json.id,
      json.name,
      json.done
    )
  }

  ToJson() {
    return <ApiPackingTask>{
      id: this.id,
      name: this.name,
      done: this.done
    }
  }

  Synchronize(otherItem: ApiPackingTask) {
    this.name = otherItem.name
    this.done = otherItem.done
  }
}
