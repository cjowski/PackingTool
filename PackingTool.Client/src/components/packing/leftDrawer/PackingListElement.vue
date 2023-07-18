<template>
  <div>
    <q-item
      :active="selected"
      clickable
      v-ripple
      dense
      :active-class="editingListName ? '' : 'selected-list'"
      :class="elementClass"
      @click="select"
      @keydown.delete.prevent="confirmDelete"
      @dblclick="editListName"
    >
      <q-item-section v-if="!editingListName">
        <q-item-label class="text-body1 text-bold label-font">
          {{ name }}
        </q-item-label>
      </q-item-section>

      <q-item-section v-else>
        <ListNameInput
          :initialListName="editedListName"
          :onSubmit="doUpdateListName"
          :onCancel="cancelEditingListName"
        ></ListNameInput>
      </q-item-section>

      <q-item-section
        v-if="!editingListName && !showDeleteListConfirmation"
        side
        @click.stop=""
      >
        <q-btn
          v-if="!showToolbar"
          flat
          dense
          icon="edit"
          @click="showToolbar = true"
        />
        <q-btn v-else flat dense icon="edit_off" @click="showToolbar = false" />
      </q-item-section>

      <q-item-section
        v-if="!editingListName && !showDeleteListConfirmation"
        side
        @click.stop=""
        style="padding-left: 4px"
      >
        <q-btn flat dense icon="close" size="md" @click="confirmDelete" />
      </q-item-section>
    </q-item>

    <q-expansion-item v-model="showToolbar" header-style="display: none">
      <q-item v-if="!showDeleteListConfirmation">
        <q-item-section>
          <q-btn
            flat
            dense
            icon="format_size"
            color="cyan-5"
            @click="editListName"
          >
            <q-tooltip>Edit name</q-tooltip>
          </q-btn>
        </q-item-section>

        <q-item-section>
          <q-btn
            flat
            dense
            icon="content_copy"
            color="purple"
            @click="copyList"
          >
            <q-tooltip>Make copy</q-tooltip>
          </q-btn>
        </q-item-section>

        <q-item-section v-if="id !== 0">
          <q-btn
            flat
            dense
            icon="download"
            color="blue-grey-2"
            @click="downloadFile"
          >
            <q-tooltip>Download</q-tooltip>
          </q-btn>
        </q-item-section>
      </q-item>

      <div v-else>
        <q-item>
          <q-item-label
            class="text-body1 text-bold text-center label-font"
            style="color: #ef9a9a"
          >
            Are you sure you want to delete list?
          </q-item-label>
        </q-item>

        <q-item>
          <q-item-section>
            <q-btn dense icon="check" color="red-5" @click="doDeleteList" />
          </q-item-section>

          <q-item-section>
            <q-btn flat dense icon="undo" @click="cancelDeleteList" />
          </q-item-section>
        </q-item>
      </div>

      <q-separator dark />
    </q-expansion-item>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref, watch } from "vue"
import { useQuasar } from "quasar"
import { storeToRefs } from "pinia"
import routerExtensions from "@/router/routerExtensions"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"
import { useOpenedPackingListStore } from "@/stores/openedPackingListStore"
import { useOperationStatusStore } from "@/stores/operationStatusStore"
import { PackingListState } from "@/models/packing/list/PackingListState"
import ListNameInput from "./AddPackingList.vue"
import { PackingSectionType } from "@/enums/PackingSectionType"
import { PackingListService } from "@/api/services/PackingListService"

const { packingListManager, setPackingListsShown } = useAllPackingListsStore()
const { clearAllSelectionsAndEdits } = useOpenedPackingListStore()
const { selectedListName } = storeToRefs(useAllPackingListsStore())
const { currentSectionFocus } = storeToRefs(useOperationStatusStore())
const $q = useQuasar()

const props = defineProps({
  id: {
    type: Number,
    required: true,
  },
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

const showToolbar = ref(false)
const showDeleteListConfirmation = ref(false)
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

const select = () => {
  currentSectionFocus.value = PackingSectionType.Lists
  clearAllSelectionsAndEdits()
  if ($q.screen.lt.md) {
    setPackingListsShown(false)
  }
  routerExtensions.pushWithLocale("list", { name: props.name })
}

const downloadFile = async () => {
  const fileName = `${props.name.toString()}.json`
  const json = await PackingListService.getApiPackingListJson({
    listId: props.id,
  })
  const fileBlob = new Blob([json], { type: "application/json" })
  const fileURL = window.URL.createObjectURL(fileBlob)
  const fileLink = document.createElement("a")
  fileLink.href = fileURL
  fileLink.setAttribute("download", fileName)
  document.body.appendChild(fileLink)
  fileLink.click()
}

const editListName = () => {
  editingListName.value = true
}

const doUpdateListName = async (name: string) => {
  editingListName.value = false
  await packingListManager.UpdateListName(props.id, name)

  if (selectedListName.value != name) {
    routerExtensions.pushWithLocale("list", { name: name })
  }
}

const cancelEditingListName = () => {
  editingListName.value = false
}

const copyList = async () => {
  const copiedListName = await packingListManager.CopyPackingList(props.name)
  routerExtensions.pushWithLocale("list", { name: copiedListName })
}

const confirmDelete = () => {
  showToolbar.value = true
  showDeleteListConfirmation.value = true
}

const cancelDeleteList = () => {
  showToolbar.value = false
  setTimeout(() => {
    showDeleteListConfirmation.value = false
  }, 500)
}

const doDeleteList = async () => {
  await packingListManager.DeleteList(props.id)
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
