const ApiClient = {
    async checkHealth() {
        const response = await fetch('/api/health');
        if (!response.ok) {
            throw new Error('Сервис недоступен.');
        }
        return response.json();
    },

    async analyzeSolution(file) {
        const formData = new FormData();
        formData.append('file', file);

        const response = await fetch('/api/analyze', {
            method: 'POST',
            body: formData
        });

        const data = await response.json().catch(() => null);

        if (!response.ok) {
            throw new Error(data?.error ?? 'Не удалось выполнить анализ.');
        }

        return data;
    }
};
