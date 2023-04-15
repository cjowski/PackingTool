<template>
  <div>
    <q-item>
      <q-item-section>
        <q-input
          v-model="newListName"
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
  initialListName: {
    type: String,
    default: "",
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

const newListName = ref(props.initialListName)
const newListNameError = ref("")

const submit = () => {
  validateNewListName()
  if (!newListNameError.value) {
    props.onSubmit(newListName.value)
  }
}

const cancel = () => {
  props.onCancel()
}

const validateNewListName = () => {
  if (!newListName.value) {
    newListNameError.value = "Please provide name"
    return
  }

  if (newListName.value === props.initialListName) {
    newListNameError.value = ""
    return
  }

  const list = allPackingLists.value.find(
    (list) => list.name === newListName.value
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
