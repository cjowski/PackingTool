<template>
  <div class="row q-pt-xl">
    <q-card class="set-temporary-password-font q-mt-xl" style="width: 400px">
      <q-card-section horizontal>
        <q-item class="col">
          <q-item-section>
            <q-item-label class="text-h5 text-bold">
              Set temporary password
            </q-item-label>
          </q-item-section>
        </q-item>
      </q-card-section>

      <q-separator dark />

      <q-card-section>
        <q-item>
          <q-item-section>
            <q-input
              v-model="temporaryPassword"
              :type="viewPassword ? 'text' : 'password'"
              outlined
              autofocus
              label="New password"
              @blur="validateTemporaryPassword"
              :error="temporaryPasswordError.length > 0"
              :error-message="temporaryPasswordError"
              class="set-temporary-password-input"
            >
              <template v-slot:append>
                <q-icon
                  :name="viewPassword ? 'visibility' : 'visibility_off'"
                  class="cursor-pointer"
                  @click="viewPassword = !viewPassword"
                />
              </template>
            </q-input>
          </q-item-section>
        </q-item>

        <q-item class="q-pb-none">
          <q-item-section>
            <q-input
              v-model="repeatTemporaryPassword"
              :type="viewPassword ? 'text' : 'password'"
              outlined
              label="Repeat new password"
              @keydown.enter.prevent="setTemporaryPassword"
              @blur="validateRepeatTemporaryPassword"
              :error="repeatTemporaryPasswordError.length > 0"
              :error-message="repeatTemporaryPasswordError"
              class="set-temporary-password-input"
            >
              <template v-slot:append>
                <q-icon
                  :name="viewPassword ? 'visibility' : 'visibility_off'"
                  class="cursor-pointer"
                  @click="viewPassword = !viewPassword"
                />
              </template>
            </q-input>
          </q-item-section>
        </q-item>
      </q-card-section>

      <q-card-actions vertical class="q-pt-none">
        <q-item class="q-pt-none">
          <q-item-section>
            <q-btn size="18px" color="primary" @click="setTemporaryPassword"
              >Submit change</q-btn
            >
          </q-item-section>
        </q-item>
      </q-card-actions>
    </q-card>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue"
import { QSpinnerHourglass, useQuasar } from "quasar"
import { UserService } from "@/api/services/UserService"
import type { SetTemporaryPassword } from "@/api"

const props = defineProps({
  userID: {
    type: Number,
    required: true,
  },
  closeChangePasswordDialog: {
    type: Function,
    required: true,
  },
})

const $q = useQuasar()

const temporaryPassword = ref("")
const repeatTemporaryPassword = ref("")

const viewPassword = ref(false)

const temporaryPasswordError = ref("")
const repeatTemporaryPasswordError = ref("")

const validateTemporaryPassword = () => {
  if (!temporaryPassword.value) {
    temporaryPasswordError.value = "Please enter new password"
    return
  }

  temporaryPasswordError.value = ""
}

const validateRepeatTemporaryPassword = () => {
  if (!repeatTemporaryPassword.value) {
    repeatTemporaryPasswordError.value = "Please repeat new password"
    return
  }

  if (repeatTemporaryPassword.value != temporaryPassword.value) {
    repeatTemporaryPasswordError.value = "Password doesn't match"
    return
  }

  repeatTemporaryPasswordError.value = ""
}

const setTemporaryPassword = async () => {
  validateTemporaryPassword()
  validateRepeatTemporaryPassword()

  if (temporaryPasswordError.value || repeatTemporaryPasswordError.value) {
    return
  }

  $q.loading.show({
    spinner: QSpinnerHourglass,
    spinnerColor: "cyan",
    spinnerSize: 140,
    message: "Changing password...",
    messageColor: "cyan",
  })

  console.log(props.userID + " " + temporaryPassword.value)

  await UserService.postApiUserSetTemporaryPassword({
    requestBody: <SetTemporaryPassword>{
      userID: props.userID,
      password: temporaryPassword.value,
    },
  })

  $q.notify({
    message: "Password changed",
    color: "blue",
    icon: "done",
  })

  $q.loading.hide()

  props.closeChangePasswordDialog()
}
</script>

<style scoped lang="scss">
.set-temporary-password-font {
  font-family: "Ink Free";
}
.set-temporary-password-input {
  :deep(.q-field__control) {
    padding: 5px 10px;
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
  :deep(.q-icon) {
    margin-top: -15px;
  }
  :deep(.q-field__messages div) {
    font-size: 16px;
    font-weight: bold;
  }
}
</style>
