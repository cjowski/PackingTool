import { computed, ref, watch } from "vue"
import { defineStore, storeToRefs } from "pinia"
import { useOpenedPackingListStore } from "./openedPackingListStore"

export const useTodoTasksStore = defineStore("todoTasks", () => {
  const { packingList } = storeToRefs(useOpenedPackingListStore())

  const editing = ref(true)

  const tasks = computed(() => packingList.value.content.tasks)
  const tasksLength = computed(() => packingList.value.content.tasks.length)

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

  watch(tasksLength, (currentTasksLength) => {
    editing.value = currentTasksLength == 0
  })

  return {
    tasks,
    editing,
    addTask,
    removeTask,
    removeAllTasks,
  }
})
