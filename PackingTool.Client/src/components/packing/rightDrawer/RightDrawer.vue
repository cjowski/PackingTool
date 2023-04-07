<template>
  <q-drawer
    v-model="modelValue"
    side="right"
    behavior="desktop"
    bordered
    overlay
    persistent
  >
    <q-scroll-area class="fit">
      <q-tabs
        v-model="selectedTab"
        dense
        class="text-grey"
        active-color="primary"
        indicator-color="primary"
        align="justify"
        narrow-indicator
      >
        <q-tab name="todo" icon="task_alt" />
        <q-tab
          v-if="importantItems.length"
          name="important"
          icon="priority_high"
        />
        <q-tab
          v-if="shoppingItems.length"
          name="shopping"
          icon="shopping_cart"
        />
        <q-tab name="notes" icon="text_snippet" />
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
    </q-scroll-area>
  </q-drawer>
</template>

<script setup lang="ts">
import { ref, watch } from "vue"
import { storeToRefs } from "pinia"
import TodoTab from "./rightDrawerTab/TodoTab.vue"
import ImportantItemsTab from "./rightDrawerTab/ImportantItemsTab.vue"
import ShoppingItemsTab from "./rightDrawerTab/ShoppingItemsTab.vue"
import NotesTab from "./rightDrawerTab/NotesTab.vue"
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
