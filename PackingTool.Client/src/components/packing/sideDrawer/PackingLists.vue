<template>
  <q-drawer
    v-model="modelValue"
    side="left"
    behavior="desktop"
    bordered
    overlay
    persistent
  >
    <q-scroll-area class="fit">
      <q-list>
        <q-item dense>
          <q-card-actions v-if="!addingList" style="width: 100%">
            <div class="col">
              <div class="row justify-end">
                <q-btn
                  icon="sync"
                  @click="packingListManager.FetchListDescriptions"
                  flat
                  dense
                  color="orange"
                  padding="xs"
                />
                <q-btn
                  icon="add"
                  @click="addingList = true"
                  flat
                  dense
                  color="cyan"
                  padding="xs"
                />
                <q-btn
                  icon="download"
                  @click="downloadAllLists"
                  flat
                  dense
                  color="blue-grey-2"
                  padding="xs"
                />
              </div>
            </div>
          </q-card-actions>
          <q-item-section v-else>
            <ListNameInput
              v-model="newListName"
              :onSubmit="addNewList"
              :onCancel="cancelAddNewList"
            ></ListNameInput>
          </q-item-section>
        </q-item>
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
import { useRoute } from "vue-router"
import router from "@/router"
import PackingListElement from "./PackingListElement.vue"
import ListNameInput from "./ListNameInput.vue"
import JSZip from "jszip"
import { storeToRefs } from "pinia"
import { usePackingListStore } from "@/stores/packingListStore"
import { PackingListService } from "@/api/services/PackingListService"

const route = useRoute()
const { packingListManager } = usePackingListStore()
const { allPackingLists } = storeToRefs(usePackingListStore())

defineProps({
  modelValue: {
    type: Boolean,
    required: true,
  },
})

const selectedListName = ref("")
const addingList = ref(false)
const newListName = ref("")

const addNewList = (name: string) => {
  router.push(`/list?name=${name}`)
  addingList.value = false
  newListName.value = ""
}

const cancelAddNewList = () => {
  addingList.value = false
  newListName.value = ""
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

packingListManager.FetchListDescriptions()
</script>
