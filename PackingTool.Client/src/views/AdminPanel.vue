<template>
  <q-page>
    <div class="row q-mt-xl">
      <q-card class="admin-panel-primary-font" style="width: 400px">
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

        <EditUser />
      </q-card>
    </div>
  </q-page>
</template>

<script setup lang="ts">
import EditUser from "@/components/admin/EditUser.vue"
</script>

<style scoped lang="scss">
.admin-panel-primary-font {
  font-family: "Ink Free";
  font-weight: bold;
}
.admin-panel-secondary-font {
  font-family: "Segoe Print";
  font-weight: bold;
}
</style>
