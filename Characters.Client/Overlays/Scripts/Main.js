/* global nfive, $, Vue, Vuex, VueGlide */
const GenderStrings = [
	"Male",
	"Female"
]

Vue.config.productionTip = false
Vue.config.devtools = true

Vue.use(Vuex)

new Vue({
	el: "main",
	store: new Vuex.Store({
		strict: true,
		modules: {
			characters: {
				state: {
					charactersList: [],
					selectedCharacterId: "",
					maximumCharacters: -1
				},

				getters: {
					characters: state => state.charactersList,
					selectedCharacter: state => state.charactersList.find(c => c.Id == state.selectedCharacterId) || null
				},

				// Triggered by $store.dispatch()
				actions: {
					async selectCharacter({ commit }, characterId) {
						// Set selected character id
						commit("setSelectedCharacter", characterId)
					},

					async deleteCharacter({ commit }, characterId) {
						// Unset selected character
						commit("setSelectedCharacter", null)
					},

					async setCharactersList({ commit }, characters) {
						// Set list of characters
						commit("setCharactersList", characters)
					}
				},

				// Triggered by commit()
				mutations: {
					// Set selected character
					setSelectedCharacter(state, characterId) {
						state.selectedCharacterId = characterId
					},

					setCharactersList(state, characters) {
						// Reset the characters list
						state.charactersList = []

						// Loop through each character to set some strings
						for (let character of characters) {
							character.GenderStr = GenderStrings[character.Gender]
							const birthDate = new Date(character.BirthDate)
							character.BirthDateStr = birthDate.getFullYear() + ' ' + birthDate.toLocaleString('sv-SE', { month: 'long' }) + ' ' + birthDate.getDate()
							state.charactersList.push(character)
						}
					}
				}
			}
		}
	}),

	render: h => h(window.CharacterSelection)
})

class BulmaModal {
	constructor(selector) {
		this.elem = document.querySelector(selector)
		this.close_data()
	}

	show() {
		this.elem.classList.toggle('is-active')
		this.on_show()
	}

	close() {
		this.elem.classList.toggle('is-active')
		this.on_close()
	}

	close_data() {
		var modalClose = this.elem.querySelectorAll("[data-bulma-modal='close'], .modal-background")
		var that = this
		modalClose.forEach(function (e) {
			e.addEventListener("click", function () {

				that.elem.classList.toggle('is-active')

				var event = new Event('modal:close')

				that.elem.dispatchEvent(event);
			})
		})
	}

	on_show() {
		var event = new Event('modal:show')

		this.elem.dispatchEvent(event);
	}

	on_close() {
		var event = new Event('modal:close')

		this.elem.dispatchEvent(event);
	}

	addEventListener(event, callback) {
		this.elem.addEventListener(event, callback)
	}
}
