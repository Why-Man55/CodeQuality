const severityLabels = {
    Error: 'Ошибка',
    Warning: 'Предупреждение',
    Info: 'Информация'
};

const severityClasses = {
    Error: 'danger',
    Warning: 'warning',
    Info: 'info'
};

const uploadForm = document.getElementById('upload-form');
const fileInput = document.getElementById('solution-file');
const fileName = document.getElementById('file-name');
const analyzeButton = document.getElementById('analyze-button');
const statusAlert = document.getElementById('status-alert');
const resultsSection = document.getElementById('results-section');
const resultsSummary = document.getElementById('results-summary');
const issuesList = document.getElementById('issues-list');
const exportActions = document.getElementById('export-actions');

function showAlert(message, type = 'danger') {
    statusAlert.textContent = message;
    statusAlert.className = `alert alert-${type}`;
    statusAlert.classList.remove('d-none');
}

function hideAlert() {
    statusAlert.classList.add('d-none');
}

function hasSelectedFile() {
    return Boolean(fileInput.files[0]);
}

function setLoading(isLoading) {
    analyzeButton.disabled = isLoading || !hasSelectedFile();
    analyzeButton.textContent = isLoading ? 'Анализ...' : 'Запустить анализ';
}

function setExportDisabled(isDisabled) {
    exportActions.querySelectorAll('button').forEach(button => {
        button.disabled = isDisabled;
    });
}

function setExportLoading(isLoading) {
    setExportDisabled(isLoading || !hasSelectedFile());
}

function formatDate(isoString) {
    return new Date(isoString).toLocaleString('ru-RU');
}

function renderIssue(issue) {
    const severity = issue.severity ?? 'Info';
    const badgeClass = severityClasses[severity] ?? 'secondary';
    const badgeLabel = severityLabels[severity] ?? severity;

    return `
        <div class="card issue-card mb-3">
            <div class="card-body">
                <div class="d-flex flex-wrap justify-content-between align-items-start gap-2 mb-2">
                    <h3 class="h6 mb-0">${escapeHtml(issue.title)}</h3>
                    <span class="badge text-bg-${badgeClass}">${badgeLabel}</span>
                </div>
                <p class="text-muted small mb-2">
                    <code>${escapeHtml(issue.ruleId)}</code>
                    · ${escapeHtml(issue.filePath)}:${issue.line}:${issue.column}
                </p>
                <p class="mb-2">${escapeHtml(issue.description)}</p>
                <p class="mb-0 recommendation">
                    <strong>Рекомендация:</strong> ${escapeHtml(issue.recommendation)}
                </p>
            </div>
        </div>
    `;
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text ?? '';
    return div.innerHTML;
}

function renderResults(result) {
    resultsSummary.innerHTML = `
        <strong>Проект:</strong> ${escapeHtml(result.projectName)}<br>
        <strong>Найдено проблем:</strong> ${result.totalIssues}<br>
        <strong>Дата анализа:</strong> ${formatDate(result.analyzedAt)}
    `;

    if (result.issues.length === 0) {
        issuesList.innerHTML = `
            <div class="alert alert-success mb-0">
                Проблем не обнаружено. Отличная работа!
            </div>
        `;
    } else {
        issuesList.innerHTML = result.issues.map(renderIssue).join('');
    }

    resultsSection.classList.remove('d-none');
}

function validateSelectedFile(file) {
    if (!file) {
        return 'Выберите файл .sln или .zip.';
    }

    if (!file.name.toLowerCase().endsWith('.sln') && !file.name.toLowerCase().endsWith('.zip')) {
        return 'Поддерживаются файлы .sln и .zip.';
    }

    return null;
}

fileInput.addEventListener('change', () => {
    const file = fileInput.files[0];
    fileName.textContent = file ? file.name : 'Файл не выбран';
    analyzeButton.disabled = !file;
    setExportDisabled(!file);
});

uploadForm.addEventListener('submit', async (event) => {
    event.preventDefault();
    hideAlert();

    const file = fileInput.files[0];
    const validationError = validateSelectedFile(file);
    if (validationError) {
        showAlert(validationError);
        return;
    }

    setLoading(true);

    try {
        const result = await ApiClient.analyzeSolution(file);
        renderResults(result);
    } catch (error) {
        resultsSection.classList.add('d-none');
        showAlert(error.message);
    } finally {
        setLoading(false);
    }
});

exportActions.addEventListener('click', async (event) => {
    const button = event.target.closest('button');
    if (!button) {
        return;
    }

    const file = fileInput.files[0];
    const validationError = validateSelectedFile(file);
    if (validationError) {
        showAlert(validationError);
        return;
    }

    const format = button.dataset.reportFormat;

    setExportLoading(true);
    hideAlert();

    try {
        const report = format
            ? await ApiClient.downloadReport(file, format)
            : await ApiClient.downloadReportsArchive(file);

        saveBlob(report.blob, report.fileName);
        showAlert('Отчет сформирован и скачан.', 'success');
    } catch (error) {
        showAlert(error.message);
    } finally {
        setExportLoading(false);
    }
});

document.addEventListener('DOMContentLoaded', async () => {
    setExportDisabled(!hasSelectedFile());

    try {
        await ApiClient.checkHealth();
    } catch {
        showAlert('Сервер недоступен. Запустите приложение и обновите страницу.');
    }
});
