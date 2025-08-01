document.addEventListener('DOMContentLoaded', function () {
    const tableBody = document.getElementById('activitiesTableBody');
    const lessonNameHeader = document.getElementById('lessonNameHeader');
    const addNewActivityBtn = document.getElementById('addNewActivityBtn');
    
    // Get lessonId from the URL query string
    const urlParams = new URLSearchParams(window.location.search);
    const lessonId = urlParams.get('lessonId');

    if (!lessonId) {
        lessonNameHeader.textContent = "Error: No Lesson ID provided.";
        return;
    }

    // Set the href for the "Add New" button
    addNewActivityBtn.href = `/Admin/ActivityEdit?lessonId=${lessonId}`;

    // Function to fetch and render the activities
    async function loadActivities() {
        try {
            // First, fetch the lesson details to display the name
            const lessonResponse = await fetch(`/api/lessons/${lessonId}`);
            if (!lessonResponse.ok) throw new Error('Lesson not found');
            const lesson = await lessonResponse.json();
            lessonNameHeader.textContent = `"${lesson.lessonName}"`;

            // Then, fetch the activities for that lesson
            const activitiesResponse = await fetch(`/api/lessons/${lessonId}/activities`);
            if (!activitiesResponse.ok) throw new Error('Could not fetch activities');
            const activities = await activitiesResponse.json();

            renderTable(activities);
        } catch (error) {
            console.error("Error loading data:", error);
            tableBody.innerHTML = `<tr><td colspan="5" class="text-center text-danger">${error.message}</td></tr>`;
        }
    }

    function renderTable(activities) {
        tableBody.innerHTML = '';
        if (!activities || activities.length === 0) {
            tableBody.innerHTML = '<tr><td colspan="5" class="text-center">No activities found for this lesson.</td></tr>';
            return;
        }

        activities.forEach(activity => {
            const row = document.createElement('tr');
            row.setAttribute('data-id', activity.activityId);
            row.innerHTML = `
                <td>${activity.activityId}</td>
                <td>${activity.title || ''}</td>
                <td>${activity.activityTypeId}</td> <!-- We can improve this later -->
                <td>${activity.sequenceOrder}</td>
                <td class="actions-column">
                    <a href="/Admin/ActivityEdit?activityId=${activity.activityId}" class="btn btn-sm btn-primary">Edit</a>
                    <button class="btn btn-sm btn-danger btn-delete">Delete</button>
                </td>
            `;
            tableBody.appendChild(row);
        });
    }
    
    // Event handler for delete
    tableBody.addEventListener('click', async function(e) {
        if (e.target.classList.contains('btn-delete')) {
            const row = e.target.closest('tr');
            const activityId = row.dataset.id;
            const activityTitle = row.cells[1].textContent;

            if (confirm(`Are you sure you want to delete the activity "${activityTitle}"?`)) {
                const response = await fetch(`/api/activities/${activityId}`, { method: 'DELETE' });
                if (response.ok) {
                    row.remove();
                    alert('Activity deleted successfully.');
                } else {
                    alert('Failed to delete activity.');
                }
            }
        }
    });

    // Initial load
    loadActivities();
});