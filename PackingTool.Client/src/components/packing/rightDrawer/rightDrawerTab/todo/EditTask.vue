<template>
  <div>
    <q-item>
      <q-item-section v-if="!editing" class="edit-todo-task-font">
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
          class="edit-todo-task-input"
        />
      </q-item-section>

      <q-item-section v-if="editing" side class="q-pa-none">
        <q-btn
          flat
          icon="undo"
          class="q-ma-none q-pa-sm"
          @click="cancelEditingTask"
        />
      </q-item-section>

      <q-item-section
        v-if="!editing"
        side
        @click.stop=""
        style="padding-left: 0px"
      >
        <q-btn flat dense icon="edit" @click="editTask" />
      </q-item-section>

      <q-item-section
        v-if="!editing"
        side
        @click.stop=""
        style="padding-left: 0px"
      >
        <q-btn flat dense icon="close" @click="removeTask(task.id)" />
      </q-item-section>
    </q-item>

    <q-item v-if="editing" class="q-pt-md q-pb-md">
      <q-btn
        color="green"
        padding="xs"
        icon="check"
        class="full-width"
        @click="submitEditedTask"
      />
    </q-item>
  </div>
</template>

<script lang="ts" setup>
import { ref } from "vue"
import type { PackingTask } from "@/models/packing/list/PackingTask"
import { useTodoTasksStore } from "@/stores/todoTasksStore"

const { removeTask } = useTodoTasksStore()

const props = defineProps({
  task: {
    type: Object as () => PackingTask,
    required: true,
  },
})

const editing = ref(false)
const editedTask = ref("")

const editTask = () => {
  editing.value = true
  editedTask.value = props.task.name
}

const submitEditedTask = () => {
  props.task.name = editedTask.value
  editing.value = false
  editedTask.value = ""
}

const cancelEditingTask = () => {
  editing.value = false
  editedTask.value = ""
}
</script>

<style lang="scss" scoped>
.edit-todo-task-font {
  font-family: "Segoe Print";
  font-weight: bold;
  font-size: 14px;
}
.edit-todo-task-input {
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
