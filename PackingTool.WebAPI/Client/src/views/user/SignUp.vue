<template>
  <q-page>
    <div class="row q-mt-md">
      <q-card style="width: 400px">
        <q-card-section horizontal>
          <q-item class="col">
            <q-item-section>
              <q-item-label class="text-capitalize text-body1 text-bold">
                Sign up to Firebase
              </q-item-label>
            </q-item-section>
          </q-item>
        </q-card-section>

        <q-separator dark />

        <q-card-section>
          <q-item>
            <q-item-section>
              <q-input
                v-model="email"
                type="email"
                standout="bg-secondary text-white"
                label="Email"
                @blur="validateEmail"
                :error="showEmailError"
                error-message="Please enter a valid email address"
              />
            </q-item-section>
          </q-item>
          <q-item>
            <q-item-section>
              <q-input
                v-model="password"
                type="password"
                standout="bg-secondary text-white"
                label="Password"
                @blur="validatePassword"
                v-on:keyup.enter="doSignUp"
                :error="showPasswordError"
                error-message="Please enter password"
              />
            </q-item-section>
          </q-item>
        </q-card-section>

        <q-card-actions vertical>
          <q-item>
            <q-item-section>
              <q-btn size="16px" color="primary" @click="doSignUp"
                >Submit</q-btn
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
import { useQuasar } from "quasar"
import router from "@/router"
import { useAuthStore } from "@/stores/authStore"

const { signUp } = useAuthStore()
const $q = useQuasar()

const email = ref("")
const password = ref("")

const showEmailError = ref(false)
const showPasswordError = ref(false)

const emailValid = () => {
  const reg =
    /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,24}))$/
  return reg.test(email.value)
}

const passwordValid = () => {
  return password.value.length > 0
}

const validateEmail = () => {
  showEmailError.value = !emailValid()
}

const validatePassword = () => {
  showPasswordError.value = !passwordValid()
}

const doSignUp = async () => {
  const _emailValid = emailValid()
  const _passwordValid = passwordValid()

  if (!_emailValid || !_passwordValid) {
    showEmailError.value = !_emailValid
    showPasswordError.value = !_passwordValid
    return
  }

  const response = await signUp(
    email.value,
    password.value
  )

  if (response.Success) {
    router.push("/")
  } else {
    $q.notify({
      message: response.Message,
      color: "negative",
    })
  }
}
</script>
