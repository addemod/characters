//var nfive = { on: function () { } }

window.CharacterSelection = {
	name: "CharacterSelection",
	template: "#character-selection-template",
	components: {
		Character: window.Character
	},
	data() {
		return {
			newCharacter: {
				firstname: "",
				lastname: "",
				gender: 0,
				birthdate: "1970-01-01"
			},
			newCharacterUnmodified: {
				firstname: "",
				lastname: "",
				gender: 0,
				birthdate: "1970-01-01"
			}
		}
	},

	computed: {
		...Vuex.mapGetters([
			"characters"
		])
	},

	mounted() {
		$("[data-toggle='modal']").click(function(e) {
			var $this = $(this)
			console.log($this)
			if ($this.attr("data-toggle") && $this.attr("data-target")) {
				var modal = new BulmaModal($this.attr("data-target"))
				modal.show()
			}
		})
		$("[data-dismiss='modal']").click(function (e) {
			var $this = $(this)
			var modal = new BulmaModal($this.attr("data-target"))
			modal.close()
		})

		// Setup character glider
		var characterGlider = new Glide('.character-select-glider', {
			//type: "carousel",
			startAt: 0,
			perView: 3,
			focusAt: "center"
		}).mount()

		nfive.on("ready", async characters => {
			await this.$store.dispatch("setCharactersList", characters)
			nfive.show()
		})

		nfive.on("syncCharacters", async characters => {
			await this.$store.dispatch('setCharactersList', characters)
		})
	},

	beforeDestroy() { },

	methods: {
		async selectCharacter(characterId) {
			await this.$store.dispatch("selectCharacter", characterId)
			nfive.send("selectCharacter", characterId)
		},

		async deleteCharacter(characterId) {
			await this.$store.dispatch("deleteCharacter", characterId)
			nfive.send("deleteCharacter", characterId)
		},

		async disconnect() {
			//$(this.$refs.disconnectModal).modal("hide")
			nfive.send("disconnect")
		},

		/*async showCreateNewCharacter() {
			var modal = $("#createNewCharacterModal")
			console.log(modal)
			var html = $("html")
			modal.addClass('is-active')
			html.addClass('is-clipped')

			modal.querySelector('.modal-background').addEventListener('click', function (e) {
				e.preventDefault();
				modal.classList.remove('is-active');
				html.classList.remove('is-clipped');
			});
		},*/

		async createNewCharacter() {
			this.newCharacter.firstname = $("#firstname").val()
			this.newCharacter.lastname = $("#lastname").val()
			this.newCharacter.gender = parseInt($(".gender-radio:checked").val())
			this.newCharacter.birthdate = $("#birthdate").val()
			console.log(this.newCharacter)
			console.log("submitCreateNewCharacter")
			nfive.send("createCharacter", this.newCharacter)

			// Copy unmodified character definition to the object we can edit
			this.newCharacter = Object.assign({}, this.newCharacterUnmodified)
		}
	}
}
