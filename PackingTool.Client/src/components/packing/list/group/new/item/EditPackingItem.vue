<template>
  <div>
    <q-item dense :class="itemClass" class="q-pa-none">
      <q-item-section>
        <q-input
          v-model="modifiedName"
          type="textarea"
          :spellcheck="false"
          autofocus
          outlined
          autogrow
          dense
          class="item-name-input"
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
            :label="modifiedCount"
            :style="modifiedCount < 10 ? 'margin-right: 4px' : ''"
          />
        </div>

        <div class="row">
          <q-btn flat dense icon="keyboard_arrow_down" @click="decreaseCount" />
        </div>
      </q-item-section>
    </q-item>

    <q-item class="q-pa-none q-pb-md">
      <q-item-section>
        <q-btn color="green" padding="xs" icon="check" @click="submit" />
      </q-item-section>

      <q-item-section>
        <q-btn flat padding="xs" icon="undo" @click="cancel" />
      </q-item-section>
    </q-item>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from "vue"
import type { PackingItem } from "@/models/packing/list/PackingItem"
import { ExistenceStatus } from "@/models/packing/list/ExistenceStatus"

const props = defineProps({
  item: {
    type: Object as () => PackingItem,
    required: true,
  },
  groupID: {
    type: Number,
    required: true,
  },
  onClosing: {
    type: Function,
    required: true,
  },
})

const modifiedName = ref(props.item.name)
const modifiedCount = ref(props.item.count)

const increaseCount = () => {
  if (modifiedCount.value < 99) ++modifiedCount.value
}

const decreaseCount = () => {
  if (modifiedCount.value > 1) --modifiedCount.value
}

const submit = () => {
  props.item.name = modifiedName.value
  props.item.count = modifiedCount.value
  props.onClosing()
}

const cancel = () => {
  props.onClosing()
}

const itemClass = computed(() => {
  let output = "q-pa-xs q-pb-none shadow-transition"
  if (props.item.status == ExistenceStatus.New) output += " new-edit-item-label"
  return output
})
</script>

<style lang="scss" scoped>
.item-name-input {
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
