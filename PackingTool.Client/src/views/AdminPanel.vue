<template>
  <q-page>
    <div class="row q-mt-xl">
      <q-card class="admin-panel-font" style="width: 400px">
        <q-card-section horizontal>
          <q-item class="col">
            <q-item-section>
              <q-item-label class="text-h5 text-bold">
                User access
              </q-item-label>
            </q-item-section>
          </q-item>
        </q-card-section>

        <q-separator dark />

        <q-card-section>
          <q-item>
            <q-item-section>
              <q-select
                clearable
                use-input
                autofocus
                outlined
                label="Username"
                v-model="selectedUser"
                :options="foundUsers"
                option-value="userID"
                option-label="userName"
                @filter="searchUsers"
                @clear="clearUsers"
                class="admin-panel-select"
              />
            </q-item-section>
          </q-item>
        </q-card-section>
      </q-card>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { ref } from "vue"
import { UserService, type UserDetails } from "@/api"

const emptyUser = <UserDetails>{ userID: 0, userName: "", authorized: false }
const selectedUser = ref(emptyUser)
const foundUsers = ref([] as UserDetails[])
const userSearchingPhrase = ref("")

const searchUsers = (
  text: string,
  doneFn: (callbackFn: () => void) => void
) => {
  userSearchingPhrase.value = text
  doneFn(async () => {
    if (userSearchingPhrase.value) {
      foundUsers.value = await UserService.getApiUserSearchUsers({
        searchingPhrase: userSearchingPhrase.value,
      })
      console.log(foundUsers.value)
    }
  })
}

const clearUsers = () => {
  foundUsers.value.length = 0
}
</script>

<style scoped lang="scss">
.admin-panel-font {
  font-family: "Ink Free";
}
.admin-panel-select {
  :deep(.q-field__input) {
    padding: 5px 0px;
  }
  :deep(.q-field__label) {
    font-size: 16px;
    font-weight: bold;
  }
  :deep(.q-field__native span) {
    font-size: 18px;
    font-weight: bold;
  }
  :deep(input) {
    padding-top: 10px;
    font-size: 18px;
    font-weight: bold;
  }
}
</style>
