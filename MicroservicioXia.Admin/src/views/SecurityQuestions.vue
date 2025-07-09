<template>
  <div>
    <v-row>
      <v-col cols="12">
        <div class="d-flex justify-space-between align-center">
          <h1 class="text-h4">Gestión de Preguntas de Seguridad</h1>
          <v-btn
            color="primary"
            @click="showCreateDialog = true"
          >
            <v-icon left>mdi-help-plus</v-icon>
            Crear Pregunta
          </v-btn>
        </div>
      </v-col>
    </v-row>

    <!-- Filtros y búsqueda -->
    <v-row class="mb-4">
      <v-col cols="12" md="6">
        <v-text-field
          v-model="search"
          label="Buscar preguntas..."
          prepend-icon="mdi-magnify"
          clearable
        ></v-text-field>
      </v-col>
      <v-col cols="12" md="3">
        <v-select
          v-model="statusFilter"
          :items="statusOptions"
          label="Filtrar por estado"
          clearable
        ></v-select>
      </v-col>
    </v-row>

    <!-- Tabla de preguntas -->
    <v-card>
      <v-data-table
        :headers="headers"
        :items="filteredQuestions"
        :loading="loading"
        :search="search"
        :items-per-page="10"
        class="elevation-1"
      >
        <template v-slot:item.isActive="{ item }">
          <v-chip
            :color="item.isActive ? 'success' : 'error'"
            small
          >
            {{ item.isActive ? 'Activa' : 'Inactiva' }}
          </v-chip>
        </template>

        <template v-slot:item.createdAt="{ item }">
          {{ formatDate(item.createdAt) }}
        </template>

        <template v-slot:item.actions="{ item }">
          <v-btn
            icon
            small
            color="primary"
            @click="editQuestion(item)"
          >
            <v-icon>mdi-pencil</v-icon>
          </v-btn>
          <v-btn
            icon
            small
            color="error"
            @click="deleteQuestion(item)"
          >
            <v-icon>mdi-delete</v-icon>
          </v-btn>
        </template>
      </v-data-table>
    </v-card>

    <!-- Diálogo para crear/editar pregunta -->
    <v-dialog v-model="showCreateDialog" max-width="600px">
      <v-card>
        <v-card-title>
          <span class="text-h5">{{ editingQuestion ? 'Editar Pregunta' : 'Crear Pregunta' }}</span>
        </v-card-title>

        <v-card-text>
          <v-form ref="form" v-model="valid">
            <v-row>
              <v-col cols="12" sm="6">
                <v-text-field
                  v-model="questionForm.questionId"
                  label="ID de Pregunta"
                  type="number"
                  :rules="[rules.required, rules.positive]"
                  required
                ></v-text-field>
              </v-col>
              <v-col cols="12">
                <v-textarea
                  v-model="questionForm.question"
                  label="Pregunta"
                  :rules="[rules.required]"
                  required
                  rows="3"
                ></v-textarea>
              </v-col>
              <v-col cols="12">
                <v-switch
                  v-model="questionForm.isActive"
                  label="Pregunta Activa"
                ></v-switch>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="blue darken-1"
            text
            @click="showCreateDialog = false"
          >
            Cancelar
          </v-btn>
          <v-btn
            color="blue darken-1"
            text
            @click="saveQuestion"
            :loading="saving"
            :disabled="!valid"
          >
            {{ editingQuestion ? 'Actualizar' : 'Crear' }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Diálogo de confirmación para eliminar -->
    <v-dialog v-model="showDeleteDialog" max-width="400px">
      <v-card>
        <v-card-title class="text-h5">Confirmar Eliminación</v-card-title>
        <v-card-text>
          ¿Está seguro de que desea eliminar la pregunta "{{ questionToDelete?.question }}"?
          Esta acción no se puede deshacer.
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="showDeleteDialog = false">
            Cancelar
          </v-btn>
          <v-btn color="error" text @click="confirmDelete" :loading="deleting">
            Eliminar
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Snackbar para mensajes -->
    <v-snackbar
      v-model="snackbar.show"
      :color="snackbar.color"
      :timeout="3000"
    >
      {{ snackbar.message }}
      <template v-slot:actions>
        <v-btn
          color="white"
          text
          @click="snackbar.show = false"
        >
          Cerrar
        </v-btn>
      </template>
    </v-snackbar>
  </div>
</template>

<script setup>
import { ref, reactive, computed, onMounted } from 'vue'
import axios from 'axios'

const loading = ref(false)
const saving = ref(false)
const deleting = ref(false)
const valid = ref(false)
const search = ref('')
const statusFilter = ref('')

const showCreateDialog = ref(false)
const showDeleteDialog = ref(false)
const editingQuestion = ref(null)
const questionToDelete = ref(null)

const questions = ref([])

const questionForm = reactive({
  questionId: '',
  question: '',
  isActive: true
})

const snackbar = reactive({
  show: false,
  message: '',
  color: 'success'
})

const headers = [
  { title: 'ID', key: 'questionId' },
  { title: 'Pregunta', key: 'question' },
  { title: 'Estado', key: 'isActive' },
  { title: 'Fecha Creación', key: 'createdAt' },
  { title: 'Acciones', key: 'actions', sortable: false }
]

const statusOptions = [
  { title: 'Activa', value: true },
  { title: 'Inactiva', value: false }
]

const rules = {
  required: v => !!v || 'Este campo es requerido',
  positive: v => v > 0 || 'El ID debe ser mayor a 0'
}

const filteredQuestions = computed(() => {
  let filtered = questions.value

  if (statusFilter.value !== '') {
    filtered = filtered.filter(question => question.isActive === statusFilter.value)
  }

  return filtered
})

const showMessage = (message, color = 'success') => {
  snackbar.message = message
  snackbar.color = color
  snackbar.show = true
}

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString('es-ES')
}

const loadQuestions = async () => {
  loading.value = true
  try {
    const response = await axios.get('/securityquestions')
    questions.value = response.data
  } catch (error) {
    console.error('Error cargando preguntas:', error)
    showMessage('Error al cargar preguntas', 'error')
  } finally {
    loading.value = false
  }
}

const resetForm = () => {
  Object.assign(questionForm, {
    questionId: '',
    question: '',
    isActive: true
  })
  editingQuestion.value = null
}

const editQuestion = (question) => {
  editingQuestion.value = question
  Object.assign(questionForm, {
    questionId: question.questionId,
    question: question.question,
    isActive: question.isActive
  })
  showCreateDialog.value = true
}

const saveQuestion = async () => {
  if (!valid.value) return

  saving.value = true
  try {
    if (editingQuestion.value) {
      // Actualizar pregunta
      await axios.put(`/securityquestions/${editingQuestion.value.id}`, questionForm)
      showMessage('Pregunta actualizada exitosamente')
    } else {
      // Crear pregunta
      await axios.post('/securityquestions', questionForm)
      showMessage('Pregunta creada exitosamente')
    }
    
    showCreateDialog.value = false
    resetForm()
    loadQuestions()
  } catch (error) {
    console.error('Error guardando pregunta:', error)
    showMessage('Error al guardar pregunta', 'error')
  } finally {
    saving.value = false
  }
}

const deleteQuestion = (question) => {
  questionToDelete.value = question
  showDeleteDialog.value = true
}

const confirmDelete = async () => {
  deleting.value = true
  try {
    await axios.delete(`/securityquestions/${questionToDelete.value.id}`)
    showMessage('Pregunta eliminada exitosamente')
    showDeleteDialog.value = false
    loadQuestions()
  } catch (error) {
    console.error('Error eliminando pregunta:', error)
    showMessage('Error al eliminar pregunta', 'error')
  } finally {
    deleting.value = false
  }
}

onMounted(() => {
  loadQuestions()
})
</script> 