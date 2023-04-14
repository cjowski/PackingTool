<template>
  <div>
    <q-item dense>
      <q-item-section>
        <q-input
          v-if="showNameInput"
          v-model="name"
          type="textarea"
          :spellcheck="false"
          autofocus
          outlined
          autogrow
          dense
          class="new-item-name-input"
          @keydown.enter.prevent="add"
        ></q-input>
      </q-item-section>

      <q-item-section side style="padding-left: 0px">
        <div class="row">
          <q-btn flat dense icon="keyboard_arrow_up" @click="increaseCount" />
        </div>

        <div class="row">
          <q-badge
            rounded
            outline
            color="indigo-2"
            :label="count"
            :style="count < 10 ? 'margin-right: 4px' : ''"
          />
        </div>

        <div class="row">
          <q-btn flat dense icon="keyboard_arrow_down" @click="decreaseCount" />
        </div>
      </q-item-section>
    </q-item>

    <q-item class="q-pt-xs">
      <q-item-section>
        <q-btn color="green" padding="xs" icon="add" :disable="!name" @click="add" />
      </q-item-section>

      <q-item-section v-if="!isNew">
        <q-btn color="orange" padding="xs" icon="sync" @click="synchronize" />
      </q-item-section>

      <q-item-section>
        <q-btn flat padding="xs" icon="undo" @click="close" />
      </q-item-section>
    </q-item>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"
import type { PackingItemAttribute } from "@/api/models/PackingItemAttribute"

const { packingListManager } = useAllPackingListsStore()
const { finishAddingItemsForGroup } = useOpenedPackingListStore()

const props = defineProps({
  groupID: {
    type: Number,
    required: true,
  },
  isNew: {
    type: Boolean,
    required: true,
  }
})

const name = ref("")
const count = ref(1)
const showNameInput = ref(true)

const increaseCount = () => {
  if (count.value < 99) ++count.value
}

const decreaseCount = () => {
  if (count.value > 1) --count.value
}

const add = () => {
  if (!name.value) return

  packingListManager.AddItem(
    name.value,
    count.value,
    [] as PackingItemAttribute[],
    props.groupID
  )
  name.value = ""
  count.value = 1
  //hack to not loose focus on input:
  showNameInput.value = false
  setTimeout(() => {
    showNameInput.value = true
  }, 10)
}

const synchronize = async () => {
  await packingListManager.SynchronizeGroup(props.groupID)
}

const close = () => {
  finishAddingItemsForGroup(props.groupID)
}
</script>

<style lang="scss" scoped>
.new-item-name-input {
  :deep(.q-field__control) {
    padding: 0 4px 0 8px;
  }
  :deep(textarea) {
    font-size: 16px;
    font-weight: bold;
  }
  font-family: "Ink Free";
}
</style>
