<template>
  <q-drawer
    v-model="modelValue"
    side="right"
    behavior="desktop"
    bordered
    persistent
    :overlay="$q.screen.lt.md"
    id="right-drawer"
  >
    <q-tabs v-model="selectedTab" narrow-indicator>
      <q-tab name="todo" icon="task_alt" class="text-orange" />
      <q-tab
        v-if="importantItems.length"
        name="important"
        icon="priority_high"
        class="text-red-12"
      />
      <q-tab
        v-if="shoppingItems.length"
        name="shopping"
        icon="shopping_cart"
        class="text-blue-5"
      />
      <q-tab name="notes" icon="text_snippet" class="text-lime-3" />
    </q-tabs>

    <q-separator />

    <q-tab-panels v-model="selectedTab" animated>
      <q-tab-panel name="todo">
        <TodoTab />
      </q-tab-panel>

      <q-tab-panel name="important">
        <ImportantItemsTab />
      </q-tab-panel>

      <q-tab-panel name="shopping">
        <ShoppingItemsTab />
      </q-tab-panel>

      <q-tab-panel name="notes">
        <NotesTab />
      </q-tab-panel>
    </q-tab-panels>
  </q-drawer>
</template>

<script setup lang="ts">
import { ref, watch } from "vue"
import { storeToRefs } from "pinia"
import TodoTab from "./rightDrawerTab/todo/TodoTab.vue"
import ImportantItemsTab from "./rightDrawerTab/important/ImportantItemsTab.vue"
import ShoppingItemsTab from "./rightDrawerTab/shopping/ShoppingItemsTab.vue"
import NotesTab from "./rightDrawerTab/notes/NotesTab.vue"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"

const {
  importantItems,
  shoppingItems,
  highlightImportantItems,
  highlightShoppingItems,
} = storeToRefs(useOpenedPackingListStore())

defineProps({
  modelValue: {
    type: Boolean,
    required: true,
  },
})

const selectedTab = ref("todo")

watch(selectedTab, (tab) => {
  highlightImportantItems.value = tab == "important"
  highlightShoppingItems.value = tab == "shopping"
})
</script>
