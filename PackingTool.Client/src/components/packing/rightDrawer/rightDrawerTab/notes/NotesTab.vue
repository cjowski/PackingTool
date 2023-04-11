<template>
  <q-scroll-area :style="`height: ${scrollAreaHeight}px`">
    <q-input
      v-model="packingList.content.notes"
      type="textarea"
      spellcheck="false"
      filled
      autogrow
      class="notes-textarea"
    />
  </q-scroll-area>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue"
import { storeToRefs } from "pinia"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"

const { packingList } = storeToRefs(useOpenedPackingListStore())

const scrollAreaHeight = ref(0)

onMounted(() => {
  scrollAreaHeight.value = window.innerHeight - 140
})
</script>

<style scoped lang="scss">
.notes-textarea {
  font-family: "Calibri";
  font-size: 16px;
}
</style>
