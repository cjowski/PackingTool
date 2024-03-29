<template>
  <q-page>
    <div class="row q-mt-xl">
      <q-card v-if="isMounted" class="login-font" style="width: 400px">
        <q-card-section horizontal>
          <q-item class="col">
            <q-item-section>
              <q-item-label class="text-h5 text-bold">{{
                $t("logIn")
              }}</q-item-label>
            </q-item-section>
          </q-item>
        </q-card-section>

        <q-separator dark />

        <q-card-section>
          <q-item>
            <q-item-section>
              <q-input
                v-model="userName"
                type="text"
                autofocus
                outlined
                :label="$t('userName')"
                @blur="validateUserName"
                :error="userNameError.length > 0"
                :error-message="$t(userNameError)"
                class="login-input"
              />
            </q-item-section>
          </q-item>
          <q-item class="q-pb-none">
            <q-item-section>
              <q-input
                v-model="password"
                :type="viewPassword ? 'text' : 'password'"
                outlined
                :label="$t('password')"
                @keydown.enter.prevent="doLogin"
                @blur="validatePassword"
                :error="passwordError.length > 0"
                :error-message="$t(passwordError)"
                class="login-input"
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
          <q-item>
            <q-item-section class="text-red text-bold text-body1">
              {{ loginError }}{{ anotherLoginError ? "... again..." : "" }}
            </q-item-section>
          </q-item>
        </q-card-section>

        <q-card-actions vertical class="q-pt-none">
          <q-item class="q-pt-none">
            <q-item-section>
              <q-btn size="18px" color="primary" @click="doLogin">{{
                $t("logInSubmit")
              }}</q-btn>
            </q-item-section>
          </q-item>
        </q-card-actions>
      </q-card>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue"
import { QSpinnerHourglass, useQuasar } from "quasar"
import routerExtensions from "@/router/routerExtensions"
import { useAuthenticationStore } from "@/stores/authenticationStore"
import { useAllPackingListsStore } from "@/stores/allPackingListsStore"

const { requiredNewPassword, tryAutoLogin, login } = useAuthenticationStore()
const { packingListManager } = useAllPackingListsStore()

const $q = useQuasar()

const isMounted = ref(false)
const userName = ref("")
const password = ref("")

const viewPassword = ref(false)

const userNameError = ref("")
const passwordError = ref("")

const loginError = ref("")
const anotherLoginError = ref(false)

const validateUserName = () => {
  if (!userName.value) {
    userNameError.value = "enterUserNameError"
    return
  }

  const reg = /^[A-Za-z0-9]*$/
  if (!reg.test(userName.value)) {
    userNameError.value = "useLettersAndNumbersError"
    return
  }

  userNameError.value = ""
}

const validatePassword = () => {
  if (!password.value) {
    passwordError.value = "enterPasswordError"
    return
  }

  passwordError.value = ""
}

const doLogin = async () => {
  validateUserName()
  validatePassword()

  if (userNameError.value || passwordError.value) {
    return
  }

  showLoadingSpinner()
  const response = await login(userName.value, password.value)

  if (response.success) {
    packingListManager.FetchListDescriptions()
    if (requiredNewPassword()) {
      routerExtensions.pushWithLocale("changePassword")
    } else {
      routerExtensions.pushWithLocale("home")
    }
  } else if (response.message! == loginError.value) {
    anotherLoginError.value = true
  } else {
    loginError.value = response.message!
    anotherLoginError.value = false
  }

  $q.loading.hide()
}

const showLoadingSpinner = () => {
  $q.loading.show({
    spinner: QSpinnerHourglass,
    spinnerColor: "cyan",
    spinnerSize: 140,
    message: "Logging...",
    messageColor: "cyan",
  })
}

onMounted(async () => {
  showLoadingSpinner()
  await tryAutoLogin().then((isSuccess) => {
    if (isSuccess) {
      packingListManager.FetchListDescriptions()
      routerExtensions.pushWithLocale("home")
    }
    isMounted.value = true
    $q.loading.hide()
  })
})
</script>

<style scoped lang="scss">
.login-font {
  font-family: "Ink Free";
}
.login-input {
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
