<template>
  <q-drawer
    v-model="modelValue"
    side="right"
    behavior="desktop"
    bordered
    overlay
    persistent
  >
    <q-scroll-area class="fit"> </q-scroll-area>
  </q-drawer>
</template>

<script setup lang="ts">
defineProps({
  modelValue: {
    type: Boolean,
    required: true,
  },
})
</script>
