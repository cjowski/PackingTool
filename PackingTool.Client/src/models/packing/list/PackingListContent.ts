import type { PackingListContent as ApiPackingListContent } from "src/api/models/PackingListContent"
import type { PackingItemType } from "src/api/models/PackingItemType"
import { PackingGroup } from "./PackingGroup"
import { PackingTask } from "./PackingTask"

export class PackingListContent implements ApiPackingListContent {
  gridColumnCount: number
  notes: string
  groups: PackingGroup[]
  tasks: PackingTask[]

  constructor(
    gridColumnCount: number,
    notes: string,
    groups: PackingGroup[],
    tasks: PackingTask[]
  ) {
    this.gridColumnCount = gridColumnCount
    this.notes = notes
    this.groups = groups
    this.tasks = tasks
  }

  static New() {
    return new PackingListContent(
      0,
      "",
      [] as PackingGroup[],
      [] as PackingTask[]
    )
  }

  ToJson() {
    return <ApiPackingListContent>{
      gridColumnCount: this.gridColumnCount,
      notes: this.notes,
      groups: this.groups.map((group) => group.ToJson()),
      tasks: this.tasks.map((task) => task.ToJson()),
    }
  }

  GetGroup(id: number) {
    return this.groups.find((group) => group.id === id)
  }

  GetTask(id: number) {
    return this.tasks.find((task) => task.id === id)
  }

  Synchronize(otherListContent: ApiPackingListContent) {
    this.gridColumnCount = otherListContent.gridColumnCount
    this.notes = otherListContent.notes

    otherListContent.groups.forEach((otherGroup) => {
      const group = this.GetGroup(otherGroup.id)
      if (!group) {
        this.groups.push(PackingGroup.FromJson(otherGroup))
      } else {
        group.Synchronize(otherGroup)
      }
    })

    otherListContent.tasks.forEach((otherTask) => {
      const task = this.GetTask(otherTask.id)
      if (!task) {
        this.tasks.push(PackingTask.FromJson(otherTask))
      } else {
        task.Synchronize(otherTask)
      }
    })

    const groupIDsToRemove = [] as number[]
    this.groups.forEach((group) => {
      if (
        !otherListContent.groups.find(
          (otherGroup) => otherGroup.id === group.id
        )
      ) {
        groupIDsToRemove.push(group.id)
      }
    })

    groupIDsToRemove.forEach((groupID) => {
      this.groups.splice(
        this.groups.findIndex((group) => group.id === groupID),
        1
      )
    })

    const taskIDsToRemove = [] as number[]
    this.tasks.forEach((task) => {
      if (
        !otherListContent.tasks.find((otherTask) => otherTask.id === task.id)
      ) {
        taskIDsToRemove.push(task.id)
      }
    })

    taskIDsToRemove.forEach((taskID) => {
      this.tasks.splice(
        this.tasks.findIndex((task) => task.id === taskID),
        1
      )
    })

    this.SortGroups()
    this.SortTasks()
  }

  AddGroup(name: string, type: PackingItemType) {
    const sort = this.groups.length
      ? Math.max.apply(
          Math,
          this.groups.map((group) => group.sort)
        ) + 1
      : 1

    const group = PackingGroup.New(this.GetNextGroupID(), name, type, sort)
    this.groups.push(group)

    if (this.gridColumnCount < 3) {
      ++this.gridColumnCount
    }

    return group
  }

  AddTask(name: string) {
    const task = PackingTask.New(this.GetNextTaskID(), name)
    this.tasks.push(task)
    return task
  }

  RemoveGroup(id: number) {
    this.groups.splice(
      this.groups.findIndex((group) => group.id === id),
      1
    )

    if (this.gridColumnCount > this.groups.length) {
      this.gridColumnCount = this.groups.length
    }
  }

  RemoveTask(id: number) {
    this.tasks.splice(
      this.tasks.findIndex((task) => task.id === id),
      1
    )
  }

  SortGroups() {
    this.groups.sort((a, b) => {
      return a.sort - b.sort
    })
  }

  SortTasks() {
    this.tasks.sort((a, b) => {
      return a.id - b.id
    })
  }

  GetNextGroupID() {
    return this.groups.length
      ? Math.max.apply(
          Math,
          this.groups.map((group) => group.id)
        ) + 1
      : 1
  }

  GetNextItemID() {
    return (
      Math.max.apply(
        Math,
        this.groups.map((group) =>
          Math.max.apply(
            Math,
            group.items.length ? group.items.map((item) => item.id) : [0]
          )
        )
      ) + 1
    )
  }

  GetNextTaskID() {
    return this.tasks.length
      ? Math.max.apply(
          Math,
          this.tasks.map((task) => task.id)
        ) + 1
      : 1
  }
}
