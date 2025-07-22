document.addEventListener('DOMContentLoaded', function() {
    const levelSelector = document.getElementById('levelSelector');
    const lessonsTableBody = document.getElementById('lessonsTableBody');
    const btnAddLesson = document.getElementById('btnAddLesson');

    // 1. Load levels into the dropdown
    async function loadLevelsDropdown() {
        try {
            const response = await fetch('/api/levels');
            const levels = await response.json();
            levelSelector.innerHTML = '<option value="">-- Select a Level --</option>';
            levels.forEach(level => {
                const option = document.createElement('option');
                option.value = level.levelId;
                option.textContent = `${level.sequenceOrder}. ${level.levelName}`;
                levelSelector.appendChild(option);
            });
        } catch (error) {
            console.error('Error loading levels dropdown:', error);
        }
    }

    // 2. Load lessons for the selected level
    async function loadLessonsForLevel(levelId) {
        if (!levelId) {
            lessonsTableBody.innerHTML = '<tr><td colspan="4">Please select a level to view lessons.</td></tr>';
            return;
        }

        try {
            const response = await fetch(`/api/lessons/level/${levelId}`);
            if (response.status === 404) {
                 lessonsTableBody.innerHTML = '<tr><td colspan="4">No lessons found for this level.</td></tr>';
                 return;
            }
            const lessons = await response.json();

            lessonsTableBody.innerHTML = '';
            lessons.forEach(lesson => {
                const row = document.createElement('tr');
                row.innerHTML = `
                    <td>${lesson.lessonId}</td>
                    <td>${lesson.lessonName}</td>
                    <td>${lesson.description || ''}</td>
                    <td>${lesson.sequenceOrder}</td>
                `;
                lessonsTableBody.appendChild(row);
            });
        } catch (error) {
            console.error('Error loading lessons:', error);
        }
    }

    // 3. Add a new lesson
    async function addNewLesson() {
        const levelId = levelSelector.value;
        const newName = document.getElementById('newLessonName').value;
        const newDesc = document.getElementById('newLessonDesc').value;
        const newOrder = document.getElementById('newLessonOrder').value;

        if (!levelId || !newName || !newOrder) {
            alert('Please select a level and fill in at least the Name and Order fields.');
            return;
        }

        const payload = {
            levelId: parseInt(levelId, 10),
            lessonName: newName,
            description: newDesc,
            sequenceOrder: parseInt(newOrder, 10)
        };

        try {
            const response = await fetch('/api/lessons', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });

            if (!response.ok) throw new Error('Failed to add lesson');

            // Clear input fields and reload lessons for the current level
            document.getElementById('newLessonName').value = '';
            document.getElementById('newLessonDesc').value = '';
            document.getElementById('newLessonOrder').value = '';
            
            await loadLessonsForLevel(levelId);
            alert('Lesson added successfully!');
        } catch (error) {
            console.error('Error adding lesson:', error);
            alert('Failed to add lesson. Check console.');
        }
    }

    // 4. Event Listeners
    levelSelector.addEventListener('change', () => loadLessonsForLevel(levelSelector.value));
    btnAddLesson.addEventListener('click', addNewLesson);

    // Initial Load
    loadLevelsDropdown();
    loadLessonsForLevel(null);
});