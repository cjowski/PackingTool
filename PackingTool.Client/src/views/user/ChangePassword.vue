<template>
  <q-page>
    <div class="row q-mt-xl">
      <q-card class="change-password-font" style="width: 400px">
        <q-card-section horizontal>
          <q-item class="col">
            <q-item-section>
              <q-item-label class="text-h5 text-bold">
                {{ $t("changePassword") }}
              </q-item-label>
            </q-item-section>
          </q-item>
        </q-card-section>

        <q-separator dark />

        <q-card-section>
          <q-item>
            <q-item-section>
              <q-input
                v-model="currentPassword"
                :type="viewCurrentPassword ? 'text' : 'password'"
                autofocus
                outlined
                :label="$t('currentPassword')"
                @blur="validateCurrentPassword"
                :error="currentPasswordError.length > 0"
                :error-message="$t(currentPasswordError)"
                class="change-password-input"
              >
                <template v-slot:append>
                  <q-icon
                    :name="
                      viewCurrentPassword ? 'visibility' : 'visibility_off'
                    "
                    class="cursor-pointer"
                    @click="viewCurrentPassword = !viewCurrentPassword"
                  />
                </template>
              </q-input>
            </q-item-section>
          </q-item>
          <q-item>
            <q-item-section>
              <q-input
                v-model="newPassword"
                :type="viewNewPassword ? 'text' : 'password'"
                outlined
                :label="$t('newPassword')"
                @blur="validateNewPassword"
                :error="newPasswordError.length > 0"
                :error-message="$t(newPasswordError)"
                class="change-password-input"
              >
                <template v-slot:append>
                  <q-icon
                    :name="viewNewPassword ? 'visibility' : 'visibility_off'"
                    class="cursor-pointer"
                    @click="viewNewPassword = !viewNewPassword"
                  />
                </template>
              </q-input>
            </q-item-section>
          </q-item>
          <q-item class="q-pb-none">
            <q-item-section>
              <q-input
                v-model="repeatNewPassword"
                :type="viewNewPassword ? 'text' : 'password'"
                outlined
                :label="$t('repeatNewPassword')"
                @keydown.enter.prevent="doChangePassword"
                @blur="validateRepeatNewPassword"
                :error="repeatNewPasswordError.length > 0"
                :error-message="$t(repeatNewPasswordError)"
                class="change-password-input"
              >
                <template v-slot:append>
                  <q-icon
                    :name="viewNewPassword ? 'visibility' : 'visibility_off'"
                    class="cursor-pointer"
                    @click="viewNewPassword = !viewNewPassword"
                  />
                </template>
              </q-input>
            </q-item-section>
          </q-item>
          <q-item v-if="requiredNewPassword()">
            <q-item-section class="text-orange text-bold text-body1"
              >{{ $t("changeTemporaryPassword") }}
            </q-item-section>
          </q-item>
        </q-card-section>

        <q-card-actions vertical class="q-pt-none">
          <q-item class="q-pt-none">
            <q-item-section>
              <q-btn size="18px" color="primary" @click="doChangePassword">{{
                $t("submitChange")
              }}</q-btn>
            </q-item-section>
          </q-item>
        </q-card-actions>
      </q-card>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { ref } from "vue"
import { QSpinnerHourglass, useQuasar } from "quasar"
import routerExtensions from "@/router/routerExtensions"
import { useAuthenticationStore } from "@/stores/authenticationStore"

const { requiredNewPassword, changePassword } = useAuthenticationStore()

const $q = useQuasar()

const currentPassword = ref("")
const newPassword = ref("")
const repeatNewPassword = ref("")

const viewCurrentPassword = ref(false)
const viewNewPassword = ref(false)

const currentPasswordError = ref("")
const newPasswordError = ref("")
const repeatNewPasswordError = ref("")

const validateCurrentPassword = () => {
  currentPasswordError.value = currentPassword.value
    ? ""
    : "enterCurrentPasswordError"
}

const validateNewPassword = () => {
  if (!newPassword.value) {
    newPasswordError.value = "enterNewPasswordError"
    return
  }

  if (newPassword.value == currentPassword.value) {
    newPasswordError.value = "setNewDifferentPasswordError"
    return
  }

  newPasswordError.value = ""
}

const validateRepeatNewPassword = () => {
  if (!repeatNewPassword.value) {
    repeatNewPasswordError.value = "repeatNewPasswordError"
    return
  }

  if (repeatNewPassword.value != newPassword.value) {
    repeatNewPasswordError.value = "repeatPasswordNotMatchedError"
    return
  }

  repeatNewPasswordError.value = ""
}

const doChangePassword = async () => {
  validateCurrentPassword()
  validateNewPassword()
  validateRepeatNewPassword()

  if (
    currentPasswordError.value ||
    newPasswordError.value ||
    repeatNewPasswordError.value
  ) {
    return
  }

  $q.loading.show({
    spinner: QSpinnerHourglass,
    spinnerColor: "cyan",
    spinnerSize: 140,
    message: "Changing password...",
    messageColor: "cyan",
  })

  const response = await changePassword(
    currentPassword.value,
    newPassword.value
  )

  if (response.success) {
    routerExtensions.pushWithLocale("home")
    $q.notify({
      message: "Password changed",
      color: "blue",
      icon: "done",
    })
  } else {
    currentPasswordError.value = response.message!
  }

  $q.loading.hide()
}
</script>

<style scoped lang="scss">
.change-password-font {
  font-family: "Ink Free";
}
.change-password-input {
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
