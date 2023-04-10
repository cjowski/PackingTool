<template>
  <div>
    <q-item class="q-pa-none q-mb-sm">
      <q-item-section>
        <q-input
          v-if="showNewTaskInput"
          v-model="newTask"
          type="textarea"
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
        <q-btn flat dense icon="check" color="green" @click="editing = false" />
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
        />
      </q-item-section>
    </q-item>

    <q-separator v-if="tasks.length != 0" dark />

    <q-scroll-area :style="`height: ${scrollAreaHeight}px`">
      <q-list>
        <q-item v-for="task in tasks" :item="task">
          <q-item-section
            v-if="editingTaskID != task.id"
            class="todo-task-font"
          >
            {{ task.name }}
          </q-item-section>
          <q-item-section v-else>
            <q-input
              v-model="editedTask"
              type="textarea"
              :spellcheck="false"
              autofocus
              outlined
              autogrow
              label="Edit task"
              @keydown.enter.prevent="submitEditedTask"
              @keydown.esc.prevent="cancelEditingTask"
              @focusout="cancelEditingTask"
              class="todo-task-input"
            />
          </q-item-section>

          <q-item-section
            v-if="editingTaskID != task.id"
            side
            @click.stop=""
            style="padding-left: 0px"
          >
            <q-btn flat dense icon="edit" @click="editTask(task)" />
          </q-item-section>

          <q-item-section
            v-if="editingTaskID != task.id"
            side
            @click.stop=""
            style="padding-left: 0px"
          >
            <q-btn flat dense icon="close" @click="removeTask(task.id)" />
          </q-item-section>
        </q-item>
      </q-list>
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
import { storeToRefs } from "pinia"
import { useTodoTasksStore } from "@/stores/todoTasksStore"
import type { PackingTask } from "@/api"

const { tasks, editing } = storeToRefs(useTodoTasksStore())
const { addTask, removeTask, removeAllTasks } = useTodoTasksStore()

const newTask = ref("")
const editedTask = ref("")
const showNewTaskInput = ref(true)
const editingTaskID = ref(0)
const showRemoveAllTasksConfirmation = ref(false)
const scrollAreaHeight = ref(0)

const addNewTask = () => {
  if (!newTask.value) return
  addTask(newTask.value)
  newTask.value = ""

  //hack to not loose focus on input:
  showNewTaskInput.value = false
  setTimeout(() => {
    showNewTaskInput.value = true
  }, 10)
}

const cancelAddingTask = () => {
  newTask.value = ""
}

const editTask = (task: PackingTask) => {
  editingTaskID.value = task.id
  editedTask.value = task.name
}

const submitEditedTask = (task: PackingTask) => {
  task.name = editedTask.value
  editingTaskID.value = 0
  editedTask.value = ""
}

const cancelEditingTask = () => {
  editingTaskID.value = 0
  editedTask.value = ""
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
  :deep(input) {
    padding-top: 10px;
    font-size: 18px;
    font-weight: bold;
  }
}
</style>
