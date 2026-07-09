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
    },

    async downloadReport(file, format) {
        const formData = new FormData();
        formData.append('file', file);

        const response = await fetch(`/api/analyze/report/${format}`, {
            method: 'POST',
            body: formData
        });

        if (!response.ok) {
            const data = await response.json().catch(() => null);
            throw new Error(data?.error ?? 'Не удалось сформировать отчет.');
        }

        return {
            blob: await response.blob(),
            fileName: getDownloadFileName(response, `code-quality-report.${format}`)
        };
    },

    async downloadReportsArchive(file) {
        const formData = new FormData();
        formData.append('file', file);

        const response = await fetch('/api/analyze/reports', {
            method: 'POST',
            body: formData
        });

        if (!response.ok) {
            const data = await response.json().catch(() => null);
            throw new Error(data?.error ?? 'Не удалось сформировать архив отчетов.');
        }

        return {
            blob: await response.blob(),
            fileName: getDownloadFileName(response, 'code-quality-reports.zip')
        };
    }
};

function getDownloadFileName(response, fallback) {
    const disposition = response.headers.get('content-disposition');
    const match = disposition?.match(/filename\*=UTF-8''([^;]+)|filename="?([^"]+)"?/i);
    const fileName = match?.[1] ?? match?.[2];

    return fileName ? decodeURIComponent(fileName) : fallback;
}

function saveBlob(blob, fileName) {
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');

    link.href = url;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    link.remove();
    URL.revokeObjectURL(url);
}
