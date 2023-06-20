<template>
  <div>
    <q-item class="q-pa-none q-mb-sm">
      <q-item-section>
        <q-input
          v-model="newTask"
          type="textarea"
          ref="newTaskInputRef"
          :spellcheck="false"
          autofocus
          outlined
          autogrow
          label="New task"
          @keydown.enter.prevent="addNewTask"
          @keydown.esc.prevent="cancelAddingTask"
          class="todo-task-input"
        />
      </q-item-section>
      <q-item-section side>
        <q-btn
          flat
          dense
          icon="add_task"
          :disable="newTask.length == 0"
          :color="newTask.length == 0 ? '' : 'green-14'"
          @click="addNewTask"
        />
      </q-item-section>
    </q-item>

    <q-item>
      <q-item-section />

      <q-item-section v-if="tasks.length != 0" side @click.stop="">
        <q-btn flat dense icon="check" color="green" @click="editing = false">
          <q-tooltip>Finish editing</q-tooltip>
        </q-btn>
      </q-item-section>

      <q-item-section
        v-if="tasks.length != 0"
        side
        @click.stop=""
        style="padding-left: 0px"
      >
        <q-btn
          flat
          dense
          icon="close"
          color="red"
          @click="showRemoveAllTasksConfirmation = true"
        >
          <q-tooltip>Delete all</q-tooltip>
        </q-btn>
      </q-item-section>
    </q-item>

    <q-separator v-if="tasks.length != 0" dark />

    <q-scroll-area :style="`height: ${scrollAreaHeight}px`">
      <EditTask v-for="task in tasks" :task="task" />
    </q-scroll-area>

    <q-dialog v-model="showRemoveAllTasksConfirmation" position="right">
      <q-card class="todo-task-font">
        <q-card-section> Do you want to remove all tasks? </q-card-section>

        <q-separator dark />

        <q-card-actions align="around">
          <q-btn flat @click="removeAllTasksAndClosePopup">Yes</q-btn>
          <q-btn flat @click="showRemoveAllTasksConfirmation = false">No</q-btn>
        </q-card-actions>
      </q-card>
    </q-dialog>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue"
import type { QInput } from "quasar"
import { storeToRefs } from "pinia"
import { useTodoTasksStore } from "@/stores/todoTasksStore"
import EditTask from "./EditTask.vue"

const { tasks, editing } = storeToRefs(useTodoTasksStore())
const { addTask, removeAllTasks } = useTodoTasksStore()

const newTask = ref("")
const showRemoveAllTasksConfirmation = ref(false)
const scrollAreaHeight = ref(0)
const newTaskInputRef = ref<InstanceType<typeof QInput>>()

const addNewTask = () => {
  if (!newTask.value) return
  addTask(newTask.value)
  newTask.value = ""
  newTaskInputRef.value?.focus()
}

const cancelAddingTask = () => {
  newTask.value = ""
}

const removeAllTasksAndClosePopup = () => {
  removeAllTasks()
  showRemoveAllTasksConfirmation.value = false
}

onMounted(() => {
  scrollAreaHeight.value = window.innerHeight - 280
})
</script>

<style lang="scss" scoped>
.todo-task-font {
  font-family: "Segoe Print";
  font-weight: bold;
  font-size: 14px;
}
.todo-task-input {
  font-family: "Ink Free";
  font-size: 16px;

  :deep(.q-field__label) {
    font-size: 16px;
    font-weight: bold;
    top: 10px;
  }
  :deep(textarea) {
    padding-top: 10px;
    font-size: 18px;
    font-weight: bold;
  }
}
</style>
