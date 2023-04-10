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
          @keydown.enter.prevent="addTask"
          @keydown.esc.prevent="cancelAddingTask"
          class="new-todo-task-input"
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
    <q-list v-if="tasks.length != 0">
      <q-item clickable>
        <q-item-section />

        <q-item-section v-if="editing" side @click.stop="">
          <q-btn
            flat
            dense
            icon="check"
            color="green"
            @click="editing = false"
          />
        </q-item-section>

        <q-item-section side @click.stop="" style="padding-left: 0px">
          <q-btn flat dense icon="close" color="red" @click="removeAllTasks" />
        </q-item-section>
      </q-item>

      <q-separator dark />

      <q-item v-for="task in tasks" :item="task">
        <q-item-section @click="task.done = !task.done">
          <div class="row q-gutter-xs non-selectable">
            <div class="col-12 q-ma-none">
              <span class="todo-task-font">{{ task.name }}</span>
            </div>
          </div>
        </q-item-section>

        <q-item-section side @click.stop="" style="padding-left: 0px">
          <q-btn flat dense icon="edit" @click="editingTask = true" />
        </q-item-section>

        <q-item-section side @click.stop="" style="padding-left: 0px">
          <q-btn flat dense icon="close" @click="removeTask(task.id)" />
        </q-item-section>
      </q-item>
    </q-list>

    <q-dialog v-model="showRemoveAllTasksConfirmation" position="right">
      <div></div>
    </q-dialog>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue"
import { storeToRefs } from "pinia"
import { useTodoTasksStore } from "@/stores/todoTasksStore"

const { tasks, editing } = storeToRefs(useTodoTasksStore())
const { addTask, removeTask, removeAllTasks } = useTodoTasksStore()

const newTask = ref("")
const showNewTaskInput = ref(true)
const editingTask = ref(false)
const showRemoveAllTasksConfirmation = ref(false)

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
</script>

<style lang="scss" scoped>
.todo-task-font {
  font-family: "Segoe Print";
  font-weight: bold;
  font-size: 14px;
}
.new-todo-task-input {
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
.todo-task-all-done-label {
  :deep(.q-checkbox__label) {
    font-family: "Ink Free";
    font-weight: bold;
    font-size: 20px;
  }
}
</style>
