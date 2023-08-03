function vision(language) {
    Alpine.data("vision", () => ({
        description: null,
        confidence: 0,
        errorMessage: null,
        isBusy: false,

        takePhoto: async function (player) {
            if (this.isBusy)
                return;

            this.description = null;
            this.errorMessage = null;

            var canvas = document.createElement("canvas");
            canvas.width = 320;
            canvas.height = 240;

            const context = canvas.getContext("2d");
            context.drawImage(player, 0, 0, canvas.width, canvas.height);

            try {
                this.isBusy = true;

                // Get base64 data to send to server for upload
                const imageBase64Data = canvas.toDataURL("image/png");
                const response = await uploadImage(imageBase64Data, language);
                const content = await response.json();

                this.isBusy = false;

                this.errorMessage = GetErrorMessage(response.status, content);

                if (this.errorMessage == null) {
                    // The request has succeeded.
                    this.description = content.caption;
                    this.confidence = content.confidence;
                }
            } catch (error) {
                this.isBusy = false;
                this.errorMessage = error;
            }
            finally
            {
                canvas = null;
            }
        }
    }));
}
