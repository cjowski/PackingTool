<template>
  <q-item
    :to="`/list?name=${name}`"
    :active="selected"
    clickable
    v-ripple
    dense
    active-class="selected-list"
    :class="elementClass"
    @click="currentSectionFocus = PackingSectionType.Lists"
    @keydown.delete.prevent="doDeleteList"
    @dblclick="editListName"
  >
    <q-item-section v-if="!editingListName">
      <q-item-label class="text-body1 text-bold label-font">
        {{ name }}
      </q-item-label>
    </q-item-section>
    <q-item-section v-else>
      <ListNameInput
        v-model="editedListName"
        :onSubmit="doUpdateListName"
        :onCancel="cancelEditingListName"
      ></ListNameInput>
    </q-item-section>
    <q-item-section v-if="!editingListName" side>
      <q-card-actions @click.prevent="" class="q-pa-xs">
        <q-btn
          v-if="state != PackingListState.New"
          icon="download"
          @click="downloadFile"
          flat
          dense
          color="blue-grey-2"
          padding="xs"
        />
        <q-btn icon="more_vert" flat dense color="blue-grey-2" padding="xs">
          <q-menu
            v-model="showMoreButtons"
            anchor="top right"
            transition-show="jump-down"
            transition-hide="jump-up"
            class="transparent no-shadow"
            :offset="[0, 25]"
          >
            <q-list dense>
              <q-item>
                <q-btn
                  icon="edit"
                  round
                  color="green"
                  size="md"
                  padding="sm"
                  @click="editListName"
                />
              </q-item>
              <q-item>
                <q-btn
                  icon="content_copy"
                  round
                  color="purple"
                  size="md"
                  padding="sm"
                  @click="copyList"
                />
              </q-item>
              <q-item>
                <q-btn
                  icon="delete"
                  round
                  color="red-5"
                  size="md"
                  padding="sm"
                  @click="doDeleteList"
                />
              </q-item>
            </q-list>
          </q-menu>
        </q-btn>
      </q-card-actions>
    </q-item-section>
  </q-item>
</template>

<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref, watch } from "vue"
import router from "@/router"
import { useQuasar } from "quasar"
import { storeToRefs } from "pinia"
import { usePackingListStore } from "@/stores/packingListStore"
import { useOperationStatusStore } from "@/stores/operationStatusStore"
import { PackingListState } from "@/models/packing/list/PackingListState"
import ListNameInput from "./ListNameInput.vue"
import { PackingSectionType } from "@/enums/PackingSectionType"
import { PackingListStorage } from "@/firebase/packingList/PackingListStorage"

const { packingListManager } = usePackingListStore()
const { selectedListName } = storeToRefs(usePackingListStore())
const { currentSectionFocus } = storeToRefs(useOperationStatusStore())
const $q = useQuasar()

const props = defineProps({
  name: {
    type: String,
    required: true,
  },
  selected: {
    type: Boolean,
    required: true,
  },
  state: {
    type: Number,
    required: true,
  },
})

const showMoreButtons = ref(false)
const editingListName = ref(false)
const editedListName = ref("")

const elementClass = computed(() => {
  switch (props.state) {
    case PackingListState.New:
      return "new-list-label"
    case PackingListState.Edited: //TODO
      return "edited-list-label"
    default:
      return ""
  }
})

const downloadFile = async () => {
  const fileName = `${props.name.toString()}.json`
  const json = await PackingListStorage.GetList(props.name)
  const jsonString = JSON.stringify(json, null, 2)
  const fileBlob = new Blob([jsonString], { type: "application/json" })
  const fileURL = window.URL.createObjectURL(fileBlob)
  const fileLink = document.createElement("a")
  fileLink.href = fileURL
  fileLink.setAttribute("download", fileName)
  document.body.appendChild(fileLink)
  fileLink.click()
}

const editListName = () => {
  editingListName.value = true
  showMoreButtons.value = false
}

const doUpdateListName = async (name: string) => {
  editingListName.value = false
  await packingListManager.UpdateListName(props.name, name)

  if (selectedListName.value != name) {
    router.push(`/list?name=${name}`)
  }
}

const cancelEditingListName = () => {
  editingListName.value = false
}

const copyList = async () => {
  const copiedListName = await packingListManager.CopyPackingList(props.name)
  router.push(`/list?name=${copiedListName}`)
}

const doDeleteList = async () => {
  showMoreButtons.value = false
  await packingListManager.DeleteList(props.name)
  $q.notify({
    message: "Deleted",
    color: "red",
    icon: "done",
  })
}

watch(editingListName, (isEditingListName) => {
  if (isEditingListName) {
    editedListName.value = props.name
  }
})

const tryCopyList = async (event: KeyboardEvent) => {
  if (
    currentSectionFocus.value == PackingSectionType.Lists &&
    props.selected &&
    event.ctrlKey &&
    event.key.toLowerCase() === "c"
  ) {
    event.preventDefault()
    await copyList()
  }
}

onMounted(() => {
  window.addEventListener("keydown", tryCopyList)
})

onUnmounted(() => {
  window.removeEventListener("keydown", tryCopyList)
})
</script>

<style lang="scss" scoped>
.new-list-label {
  div {
    color: #c6ff00; //lime-13
    font-weight: bold;
  }
}
.edited-list-label {
  div {
    color: purple;
    font-weight: bold;
  }
}
.label-font {
  font-family: "Ink Free";
}
.selected-list {
  background: $selected;
  color: $selected-text;
}
</style>
