<template>
  <div>
    <EditTasksVue v-if="editing" />
    <CheckTasks v-else />
  </div>
</template>

<script setup lang="ts">
import { storeToRefs } from "pinia"
import CheckTasks from "./CheckTasks.vue"
import EditTasksVue from "./EditTasks.vue"
import { useTodoTasksStore } from "@/stores/todoTasksStore"

const { editing } = storeToRefs(useTodoTasksStore())
</script>

<style lang="scss" scoped></style>
