<template>
  <q-input
    v-model="packingList.content.notes"
    type="textarea"
    spellcheck="false"
    filled
    autogrow
    class="notes-textarea"
  />
</template>

<script setup lang="ts">
import { storeToRefs } from "pinia"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"

const { packingList } = storeToRefs(useOpenedPackingListStore())
</script>

<style scoped lang="scss">
.notes-textarea {
  font-family: "Calibri";
  font-size: 16px;
}
</style>
