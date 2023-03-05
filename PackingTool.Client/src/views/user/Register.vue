<template>
  <q-page>
    <div class="row q-mt-md">
      <q-card class="register-font" style="width: 400px">
        <q-card-section horizontal>
          <q-item class="col">
            <q-item-section>
              <q-item-label class="text-h5 text-bold"> Register </q-item-label>
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
                label="Username"
                @blur="validateUserName"
                :error="userNameError.length > 0"
                :error-message="userNameError"
                class="register-input"
              />
            </q-item-section>
          </q-item>
          <q-item>
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
                type="password"
                outlined
                label="Password"
                v-on:keyup.enter="doRegister"
                @blur="validatePassword"
                :error="passwordError.length > 0"
                :error-message="passwordError"
                class="register-input"
              />
            </q-item-section>
          </q-item>
          <q-item>
            <q-item-section class="text-red text-bold text-body1">
              {{ registerError }}
            </q-item-section>
          </q-item>
        </q-card-section>

        <q-card-actions vertical class="q-pt-none">
          <q-item class="q-pt-none">
            <q-item-section>
              <q-btn size="18px" color="primary" @click="doRegister"
                >Register</q-btn
              >
            </q-item-section>
          </q-item>
        </q-card-actions>
      </q-card>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import { ref } from "vue"
import router from "@/router"
import { useAuthenticationStore } from "@/stores/authenticationStore"

const { register } = useAuthenticationStore()

const userName = ref("")
const email = ref("")
const password = ref("")

const userNameError = ref("")
const emailError = ref("")
const passwordError = ref("")

const registerError = ref("")

const validateUserName = () => {
  if (!userName.value) {
    userNameError.value = "Please enter username"
    return
  }

  const reg = /^[A-Za-z0-9]*$/
  if (!reg.test(userName.value)) {
    userNameError.value = "Please use only letters and numbers"
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
    passwordError.value = "Please enter password"
    return
  }

  passwordError.value = ""
}

const doRegister = async () => {
  validateUserName()
  validateEmail()
  validatePassword()

  if (userNameError.value || emailError.value || passwordError.value) {
    return
  }

  const response = await register(userName.value, email.value, password.value)

  if (response.success) {
    router.push("/")
  } else {
    registerError.value = response.message!
  }
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
  :deep(.q-field__messages) {
    font-size: 16px;
    font-weight: bold;
  }
}
</style>
