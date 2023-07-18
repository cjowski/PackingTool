<template>
  <q-drawer
    v-model="modelValue"
    side="left"
    behavior="desktop"
    bordered
    persistent
    :overlay="$q.screen.lt.md"
  >
    <q-scroll-area class="fit">
      <q-list>
        <q-item v-if="!addingList">
          <q-item-section>
            <q-btn
              icon="playlist_add"
              @click="addingList = true"
              flat
              dense
              color="green-14"
              padding="xs"
            >
              <q-tooltip>Add list</q-tooltip>
            </q-btn>
          </q-item-section>

          <q-item-section>
            <q-btn
              icon="sync"
              @click="packingListManager.FetchListDescriptions"
              flat
              dense
              color="orange"
              padding="xs"
            >
              <q-tooltip>Refresh lists</q-tooltip>
            </q-btn>
          </q-item-section>

          <q-item-section>
            <input
              style="display: none"
              type="file"
              accept="application/json"
              ref="uploadJsonFile"
              @change="tryUploadList"
            />
            <q-btn
              icon="upload"
              @click="chooseFileToUpload"
              flat
              dense
              color="blue-grey-2"
              padding="xs"
            >
              <q-tooltip>Upload list from .json file</q-tooltip>
            </q-btn>
          </q-item-section>

          <q-item-section>
            <q-btn
              v-if="allPackingLists.length > 0"
              icon="download"
              @click="downloadAllLists"
              flat
              dense
              color="blue-grey-2"
              padding="xs"
            >
              <q-tooltip>Download lists</q-tooltip>
            </q-btn>
          </q-item-section>
        </q-item>

        <AddPackingList
          v-else
          :onSubmit="addNewList"
          :onCancel="cancelAddNewList"
        />

        <q-separator />

        <template v-for="list in allPackingLists">
          <PackingListElement
            :id="list.id"
            :name="list.name"
            :selected="list.name == selectedListName"
            :state="list.state"
          ></PackingListElement>
        </template>
      </q-list>
    </q-scroll-area>
  </q-drawer>
</template>

<script setup lang="ts">
import { ref, watch } from "vue"
import { useQuasar } from "quasar"
import { useRoute } from "vue-router"
import routerExtensions from "@/router/routerExtensions"
import PackingListElement from "./PackingListElement.vue"
import AddPackingList from "./AddPackingList.vue"
import JSZip from "jszip"
import { storeToRefs } from "pinia"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"
import { PackingListService } from "@/api/services/PackingListService"

const $q = useQuasar()
const route = useRoute()
const { packingListManager } = useAllPackingListsStore()
const { allPackingLists } = storeToRefs(useAllPackingListsStore())

defineProps({
  modelValue: {
    type: Boolean,
    required: true,
  },
})

const selectedListName = ref("")
const addingList = ref(false)
const uploadJsonFile = ref<HTMLLinkElement>()

const addNewList = (name: string) => {
  routerExtensions.pushWithLocale("list", { name: name })
  addingList.value = false
}

const cancelAddNewList = () => {
  addingList.value = false
}

const chooseFileToUpload = () => {
  uploadJsonFile.value!.click()
}

const tryUploadList = async (event: Event) => {
  const selectedFile = (<HTMLInputElement>event.target).files![0]

  if (selectedFile.type != "application/json") {
    $q.notify({
      message: "Invalid file type. Please upload only JSON files.",
      color: "red",
      icon: "error",
    })
    return
  }

  if (!(await packingListManager.TryAddListFromJsonFile(selectedFile))) {
    $q.notify({
      message:
        "Oups something went wrong ;( Please check if JSON file is correct.",
      color: "red",
      icon: "error",
    })
  }
}

const downloadAllLists = async () => {
  const zip = new JSZip()
  for (const list of allPackingLists.value) {
    const json = await PackingListService.getApiPackingListJson({
      listId: list.id,
    })
    const fileBlob = new Blob([json], { type: "application/json" })
    zip.file(`${list.name}.json`, fileBlob)
  }
  zip.generateAsync({ type: "base64" }).then((content) => {
    location.href = "data:application/zip;base64," + content
  })
}

watch(
  route,
  (to) => {
    if (to.query.name) {
      selectedListName.value = to.query.name.toString()
    }
  },
  { flush: "pre", immediate: true, deep: true }
)
</script>
