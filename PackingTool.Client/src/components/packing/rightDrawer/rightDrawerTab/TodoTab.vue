<template>
  <div>
    <q-input
      v-model="newTask"
      autofocus
      outlined
      label="New task"
      @keydown.enter.prevent="addTask"
      @keydown.esc.prevent="cancelAddingTask"
      @focusout="cancelAddingTask"
      class="new-todo-task-input"
    ></q-input>
    <q-list>
      <q-item
        v-if="tasks.length > 1"
        clickable
        @click="finishAllTasks(!allTasksDone)"
      >
        <q-item-section>
          <q-checkbox dense v-model="allTasksDone" color="primary" keep-color />
        </q-item-section>
        <q-item-section side @click.stop="">
          <q-btn flat dense icon="close" @click="removeAllTasks" />
        </q-item-section>
      </q-item>

      <q-separator v-if="tasks.length > 1" dark />

      <q-item v-for="task in tasks" :item="task" clickable>
        <q-item-section side>
          <q-checkbox dense v-model="task.done" color="amber-10" keep-color />
        </q-item-section>

        <q-item-section @click="task.done = !task.done">
          <div class="row q-gutter-xs non-selectable">
            <div class="col-12 q-ma-none">
              <span class="todo-task-font">{{ task.name }}</span>
            </div>
          </div>
        </q-item-section>

        <q-item-section side @click.stop="">
          <q-btn flat dense icon="close" @click="removeTask(task.id)" />
        </q-item-section>
      </q-item>
    </q-list>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from "vue"
import { storeToRefs } from "pinia"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"

const { packingList } = storeToRefs(useOpenedPackingListStore())

const newTask = ref("")

const tasks = computed(() => packingList.value.content.tasks)

const allTasksDone = computed({
  get() {
    return tasks.value.every((task) => task.done)
  },
  set(value: boolean) {
    finishAllTasks(value)
  },
})

const finishAllTasks = (value: boolean) => {
  tasks.value.forEach((task) => (task.done = value))
}

const addTask = () => {
  if (!newTask.value) return
  packingList.value.content.AddTask(newTask.value)
  newTask.value = ""
}

const removeTask = (taskID: number) => {
  packingList.value.content.RemoveTask(taskID)
}

const removeAllTasks = () => {
  const taskIDs = tasks.value.map((task) => task.id)
  taskIDs.forEach((taskID) => packingList.value.content.RemoveTask(taskID))
}

const cancelAddingTask = () => {
  newTask.value = ""
}
</script>

<style lang="scss" scoped>
.todo-task-font {
  font-family: "Segoe Print";
  font-weight: bold;
  font-size: 15px;
}
.new-todo-task-input {
  font-family: "Ink Free";
  font-size: 16px;

  :deep(.q-field__control) {
    padding: 0 10px;
    height: 50px;
  }
  :deep(.q-field__label) {
    font-size: 16px;
    font-weight: bold;
    top: 10px;
  }
  :deep(input) {
    padding-top: 10px;
    font-size: 18px;
    font-weight: bold;
  }
}
</style>
