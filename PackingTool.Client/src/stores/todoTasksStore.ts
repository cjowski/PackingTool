import { computed, ref, watch } from "vue"
import { defineStore, storeToRefs } from "pinia"
import { useOpenedPackingListStore } from "./openedPackingListStore"
import { useAllPackingListsStore } from "./allPackingListsStore"

export const useTodoTasksStore = defineStore("todoTasks", () => {
  const { selectedListName } = storeToRefs(useAllPackingListsStore())
  const { packingList } = storeToRefs(useOpenedPackingListStore())

  const editing = ref(false)

  const tasks = computed(() => packingList.value.content.tasks)

  const addTask = (taskName: string) => {
    packingList.value.content.AddTask(taskName)
  }

  const removeTask = (taskID: number) => {
    packingList.value.content.RemoveTask(taskID)
  }

  const removeAllTasks = () => {
    const taskIDs = tasks.value.map((task) => task.id)
    taskIDs.forEach((taskID) => packingList.value.content.RemoveTask(taskID))
  }

  watch(selectedListName, () => {
    editing.value = tasks.value.length == 0
  })

  return {
    tasks,
    editing,
    addTask,
    removeTask,
    removeAllTasks,
  }
})
