<template>
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
          :options="displayedUsers"
          option-value="userID"
          option-label="userName"
          @filter="searchUsers"
          @clear="clearUsers"
          @remove="clearUsers"
          @popup-show="enableButtons = false"
          @popup-hide="enableButtons = true"
          class="search-user-select"
        >
          <template v-slot:option="scope">
            <q-item v-bind="scope.itemProps">
              <q-item-section>
                <q-item-label class="search-user-select-option-label">{{
                  scope.opt.userName
                }}</q-item-label>
              </q-item-section>
            </q-item>
          </template>
        </q-select>
      </q-item-section>
    </q-item>
    <q-card-actions horizontal v-if="selectedUser?.userID > 0">
      <q-btn
        v-if="selectedUser?.authorized"
        color="red"
        class="full-width"
        :disable="!enableButtons"
        @click="unauthorizeUser"
      >
        Unauthorize
      </q-btn>
      <q-btn
        v-else
        color="green"
        class="full-width"
        :disable="!enableButtons"
        @click="authorizeUser"
      >
        Authorize
      </q-btn>
      <q-btn
        color="primary"
        class="full-width q-mt-md"
        :disable="!enableButtons"
        @click="setTemporaryPassword = true"
      >
        Set temporary password
      </q-btn>
    </q-card-actions>
    <q-dialog v-model="setTemporaryPassword" position="top">
      <SetTemporaryPassword
        :userID="selectedUser.userID"
        :closeChangePasswordDialog="
          () => {
            setTemporaryPassword = false
          }
        "
      />
    </q-dialog>
  </q-card-section>
</template>

<script setup lang="ts">
import { computed, ref } from "vue"
import { UserService, type UserDetails } from "@/api"
import { useAuthenticationStore } from "@/stores/authenticationStore"
import SetTemporaryPassword from "@/components/admin/SetTemporaryPassword.vue"

const { userID } = useAuthenticationStore()

const foundUsers = ref([] as UserDetails[])
const userSearchingPhrase = ref("")

const emptyUser = <UserDetails>{ userID: 0, userName: "", authorized: false }
const selectedUser = ref(emptyUser)

const enableButtons = ref(true)
const setTemporaryPassword = ref(false)

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
    }
  })
}

const displayedUsers = computed(() => {
  return foundUsers.value.filter((user) => user.userID != userID())
})

const clearUsers = () => {
  foundUsers.value.length = 0
  selectedUser.value = emptyUser
}

const authorizeUser = async () => {
  await UserService.postApiUserAuthorizeUser({
    userId: selectedUser.value.userID,
    authorized: true,
  })
  selectedUser.value.authorized = true
}

const unauthorizeUser = async () => {
  await UserService.postApiUserAuthorizeUser({
    userId: selectedUser.value.userID,
    authorized: false,
  })
  selectedUser.value.authorized = false
}
</script>

<style scoped lang="scss">
.search-user-select {
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
.search-user-select-option-label {
  font-family: "Ink Free";
  font-weight: bold;
  font-size: 18px;
}
</style>
