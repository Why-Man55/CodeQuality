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

function showAlert(message, type = 'danger') {
    statusAlert.textContent = message;
    statusAlert.className = `alert alert-${type}`;
    statusAlert.classList.remove('d-none');
}

function hideAlert() {
    statusAlert.classList.add('d-none');
}

function setLoading(isLoading) {
    analyzeButton.disabled = isLoading;
    analyzeButton.textContent = isLoading ? 'Анализ...' : 'Запустить анализ';
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
    div.textContent = text;
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

fileInput.addEventListener('change', () => {
    const file = fileInput.files[0];
    fileName.textContent = file ? file.name : 'Файл не выбран';
    analyzeButton.disabled = !file;
});

uploadForm.addEventListener('submit', async (event) => {
    event.preventDefault();
    hideAlert();

    const file = fileInput.files[0];
    if (!file) {
        showAlert('Выберите файл .sln или .zip.');
        return;
    }

    if (!file.name.toLowerCase().endsWith('.sln') && !file.name.toLowerCase().endsWith('.zip')) {
        showAlert('Поддерживаются файлы .sln и .zip.');
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

document.addEventListener('DOMContentLoaded', async () => {
    try {
        await ApiClient.checkHealth();
    } catch {
        showAlert('Сервер недоступен. Запустите приложение и обновите страницу.');
    }
});
