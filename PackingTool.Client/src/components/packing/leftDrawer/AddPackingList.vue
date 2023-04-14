<template>
  <div>
    <q-item>
      <q-item-section>
        <q-input
          v-model="modelValue"
          autofocus
          outlined
          label="Name"
          class="q-pa-none q-ma-none drawer-list-name-input"
          @keydown.delete.stop=""
          @keydown.enter.prevent="submit"
          @keydown.esc.prevent="cancel"
          :error="!!newListNameError"
          :error-message="newListNameError"
        />
      </q-item-section>
      <q-item-section side class="q-pa-none">
        <q-btn flat icon="undo" class="q-ma-none q-pa-sm" @click="cancel" />
      </q-item-section>
    </q-item>
    
    <q-item class="q-pt-md q-pb-md">
      <q-btn
        color="green"
        padding="xs"
        icon="check"
        class="full-width"
        @click="submit"
      />
    </q-item>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue"
import { storeToRefs } from "pinia"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"

const { allPackingLists } = storeToRefs(useAllPackingListsStore())

const props = defineProps({
  modelValue: {
    type: String,
    required: true,
  },
  onSubmit: {
    type: Function,
    required: true,
  },
  onCancel: {
    type: Function,
    required: true,
  },
})

const initialListName = ref(props.modelValue)
const newListNameError = ref("")

const submit = () => {
  validateNewListName()
  if (!newListNameError.value) {
    props.onSubmit(props.modelValue)
  }
}

const cancel = () => {
  props.onCancel()
}

const validateNewListName = () => {
  if (!props.modelValue) {
    newListNameError.value = "Please provide name"
    return
  }

  if (props.modelValue === initialListName.value) {
    newListNameError.value = ""
    return
  }

  const list = allPackingLists.value.find(
    (list) => list.name === props.modelValue
  )
  if (list) {
    newListNameError.value = "Already exist list with given name"
    return
  }

  newListNameError.value = ""
}
</script>

<style scoped lang="scss">
.drawer-list-name-input {
  :deep(.q-field__control) {
    padding: 0 10px;
    height: 50px;
  }
  :deep(.q-field__label) {
    font-size: 16px;
    top: 10px;
    font-weight: bold;
  }
  :deep(input) {
    padding-top: 10px;
    font-size: 18px;
    font-weight: bold;
  }
  font-family: "Ink Free";
}
</style>
