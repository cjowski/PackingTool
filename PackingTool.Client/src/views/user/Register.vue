<template>
  <q-page>
    <div class="row q-mt-xl">
      <q-card class="register-font" style="width: 400px">
        <q-card-section horizontal>
          <q-item class="col">
            <q-item-section>
              <q-item-label class="text-h5 text-bold">
                {{ $t("register") }}
              </q-item-label>
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
                class="register-input"
              />
            </q-item-section>
          </q-item>
          <q-item v-if="false">
            <q-item-section>
              <q-input
                v-model="email"
                type="email"
                outlined
                label="Email"
                @blur="validateEmail"
                :error="emailError.length > 0"
                :error-message="emailError"
                class="register-input"
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
                @keydown.enter.prevent="doRegister"
                @blur="validatePassword"
                :error="passwordError.length > 0"
                :error-message="$t(passwordError)"
                class="register-input"
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
            <q-item-section
              v-if="registerSucceed"
              class="text-light-green-14 text-bold text-body1"
            >
              {{ $t("successfullyRegistered") }}!<br />{{
                $t("askAdministratorToAuthorizeUser")
              }}.
            </q-item-section>
            <q-item-section v-else class="text-red text-bold text-body1">
              {{ registerError }}
            </q-item-section>
          </q-item>
        </q-card-section>

        <q-card-actions vertical class="q-pt-none">
          <q-item class="q-pt-none">
            <q-item-section>
              <q-btn size="18px" color="primary" @click="doRegister">{{
                $t("registerSubmit")
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
import { useAuthenticationStore } from "@/stores/authenticationStore"

const { register } = useAuthenticationStore()

const $q = useQuasar()

const userName = ref("")
const email = ref("")
const password = ref("")

const viewPassword = ref(false)

const userNameError = ref("")
const emailError = ref("")
const passwordError = ref("")

const registerError = ref("")
const registerSucceed = ref(false)

const validateUserName = () => {
  if (!userName.value) {
    userNameError.value = "enterUserNameError"
    return
  }

  if (userName.value.length < 5) {
    userNameError.value = "userNameAtLeast5CharactersError"
    return
  }

  if (userName.value.length > 20) {
    userNameError.value = "userNameMax20CharactersError"
    return
  }

  const reg = /^[A-Za-z0-9]*$/
  if (!reg.test(userName.value)) {
    userNameError.value = "useLettersAndNumbersError"
    return
  }

  userNameError.value = ""
}

const validateEmail = () => {
  if (!email.value) {
    emailError.value = "Please enter email address"
    return
  }

  const reg =
    /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,24}))$/
  if (!reg.test(email.value)) {
    emailError.value = "Please enter valid email address"
    return
  }

  emailError.value = ""
}

const validatePassword = () => {
  if (!password.value) {
    passwordError.value = "enterPasswordError"
    return
  }

  if (password.value.length < 5) {
    passwordError.value = "passwordAtLeast5CharactersError"
    return
  }

  if (password.value.length > 20) {
    passwordError.value = "passwordMax20CharactersError"
    return
  }

  passwordError.value = ""
}

const doRegister = async () => {
  validateUserName()
  validateEmail()
  validatePassword()

  if (userNameError.value || passwordError.value) {
    return
  }

  $q.loading.show({
    spinner: QSpinnerHourglass,
    spinnerColor: "cyan",
    spinnerSize: 140,
    message: "Register...",
    messageColor: "cyan",
  })

  const response = await register(userName.value, password.value)

  if (response.success) {
    registerSucceed.value = true
  } else {
    registerError.value = response.message!
  }

  $q.loading.hide()
}
</script>

<style scoped lang="scss">
.register-font {
  font-family: "Ink Free";
}
.register-input {
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
