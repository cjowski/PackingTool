<template>
  <div>
    <EditTasksVue v-if="editing || tasks.length == 0" />
    <CheckTasks v-else />
  </div>
</template>

<script setup lang="ts">
import { storeToRefs } from "pinia"
import CheckTasks from "./CheckTasks.vue"
import EditTasksVue from "./EditTasks.vue"
import { useTodoTasksStore } from "@/stores/todoTasksStore"

const { tasks, editing } = storeToRefs(useTodoTasksStore())
</script>

<style lang="scss" scoped></style>
