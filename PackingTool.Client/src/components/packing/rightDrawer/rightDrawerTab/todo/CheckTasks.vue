<template>
  <div>
    <q-item clickable @click="finishAllTasks(!allTasksDone)">
      <q-item-section>
        <q-checkbox
          v-if="tasks.length > 1"
          dense
          v-model="allTasksDone"
          color="primary"
          keep-color
          class="todo-task-all-done-label"
          >All done</q-checkbox
        >
      </q-item-section>

      <q-item-section side @click.stop="">
        <q-btn
          flat
          dense
          icon="edit"
          color="yellow-5"
          @click="editing = true"
        />
      </q-item-section>
    </q-item>

    <q-separator dark />

    <q-scroll-area :style="`height: ${scrollAreaHeight}px`">
      <q-list>
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
        </q-item>
      </q-list>
    </q-scroll-area>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from "vue"
import { storeToRefs } from "pinia"
import { useTodoTasksStore } from "@/stores/todoTasksStore"

const { tasks, editing } = storeToRefs(useTodoTasksStore())

const scrollAreaHeight = ref(0)

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

onMounted(() => {
  scrollAreaHeight.value = window.innerHeight - 180
})
</script>

<style lang="scss" scoped>
.todo-task-font {
  font-family: "Segoe Print";
  font-weight: bold;
  font-size: 14px;
}
.todo-task-all-done-label {
  :deep(.q-checkbox__label) {
    font-family: "Ink Free";
    font-weight: bold;
    font-size: 20px;
  }
}
</style>