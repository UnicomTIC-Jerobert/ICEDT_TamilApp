document.addEventListener('DOMContentLoaded', function () {
    // --- FORM ELEMENTS ---
    const form = document.getElementById('activityForm');
    const pageHeader = document.getElementById('pageHeader');
    const activityIdInput = document.getElementById('activityId');
    const lessonIdInput = document.getElementById('lessonId');
    const titleInput = document.getElementById('title');
    const mainActivitySelect = document.getElementById('mainActivityId');
    const activityTypeSelect = document.getElementById('activityTypeId');
    const sequenceOrderInput = document.getElementById('sequenceOrder');
    const contentJsonTextarea = document.getElementById('contentJson');
    const saveBtn = document.getElementById('saveBtn');
    const backLink = document.getElementById('backLink');

    // --- STATE MANAGEMENT ---
    const urlParams = new URLSearchParams(window.location.search);
    const activityId = urlParams.get('activityId');
    const lessonId = urlParams.get('lessonId');
    const isEditMode = activityId !== null;

    // --- API HELPER FUNCTIONS ---

    // Generic function to populate a <select> dropdown
    async function populateDropdown(selectElement, apiUrl, valueField, textField) {
        try {
            const response = await fetch(apiUrl);
            if (!response.ok) throw new Error(`Failed to fetch from ${apiUrl}`);
            const items = await response.json();

            selectElement.innerHTML = `<option value="">-- Select an option --</option>`;
            items.forEach(item => {
                const option = document.createElement('option');
                option.value = item[valueField];
                option.textContent = item[textField];
                selectElement.appendChild(option);
            });
        } catch (error) {
            console.error(`Error populating dropdown for ${selectElement.id}:`, error);
            selectElement.innerHTML = `<option value="">-- Error loading data --</option>`;
        }
    }

    // Fetches the details of a single activity for Edit mode
    async function fetchActivityDetails(id) {
        try {
            const response = await fetch(`/api/activities/${id}`);
            if (!response.ok) throw new Error('Activity not found');
            return await response.json();
        } catch (error) {
            console.error('Error fetching activity details:', error);
            alert('Could not load activity data. Redirecting back to the list.');
            window.location.href = document.referrer || '/Admin/Lessons';
        }
    }

    // --- FORM LOGIC ---

    // Populates the entire form with data for an existing activity
    function populateForm(activity) {
        activityIdInput.value = activity.activityId;
        lessonIdInput.value = activity.lessonId; // Important for the back link
        titleInput.value = activity.title;
        sequenceOrderInput.value = activity.sequenceOrder;

        // Pretty-print the JSON for better readability
        try {
            contentJsonTextarea.value = JSON.stringify(JSON.parse(activity.contentJson), null, 2);
        } catch {
            contentJsonTextarea.value = activity.contentJson; // Fallback to raw text if not valid JSON
        }

        // Set the selected option in the dropdowns
        mainActivitySelect.value = activity.mainActivityId;
        activityTypeSelect.value = activity.activityTypeId;

        // Set the back link to go to the correct lesson's activity list
        backLink.href = `/Admin/Activities?lessonId=${activity.lessonId}`;
    }

    // Handles the form submission for both Create (POST) and Update (PUT)
    async function handleFormSubmit(event) {
        event.preventDefault(); // Prevent default browser form submission
        saveBtn.disabled = true;
        saveBtn.textContent = 'Saving...';

        let contentJsonValue = contentJsonTextarea.value;
        // Validate JSON before submitting
        try {
            JSON.parse(contentJsonValue);
        } catch (error) {
            alert('Content JSON is not valid. Please correct it before saving.');
            contentJsonTextarea.focus();
            saveBtn.disabled = false;
            saveBtn.textContent = 'Save Changes';
            return;
        }

        const payload = {
            lessonId: parseInt(lessonIdInput.value, 10),
            title: titleInput.value,
            mainActivityId: parseInt(mainActivitySelect.value, 10),
            activityTypeId: parseInt(activityTypeSelect.value, 10),
            sequenceOrder: parseInt(sequenceOrderInput.value, 10),
            contentJson: contentJsonValue
        };

        const url = isEditMode ? `/api/activities/${activityId}` : '/api/activities';
        const method = isEditMode ? 'PUT' : 'POST';

        try {
            const response = await fetch(url, {
                method: method,
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.title || `Failed to save the activity.`);
            }

            alert(`Activity ${isEditMode ? 'updated' : 'created'} successfully!`);
            window.location.href = backLink.href; // Redirect back to the list page

        } catch (error) {
            console.error('Error saving activity:', error);
            alert(`Error: ${error.message}`);
            saveBtn.disabled = false;
            saveBtn.textContent = 'Save Changes';
        }
    }

    // --- INITIALIZATION LOGIC ---

    async function initializePage() {
        // Start loading all dropdowns in parallel for speed
        const dropdownPromises = [
            populateDropdown(mainActivitySelect, '/api/mainactivities', 'id', 'name'),
            populateDropdown(activityTypeSelect, '/api/activitytypes', 'id', 'name')
        ];

        if (isEditMode) {
            pageHeader.textContent = `Edit Activity (ID: ${activityId})`;

            // Wait for dropdowns to load AND for activity details to load
            const [_, __, activity] = await Promise.all([...dropdownPromises, fetchActivityDetails(activityId)]);

            if (activity) {
                populateForm(activity);
            }
        } else {
            pageHeader.textContent = 'Add New Activity';

            // Set the hidden lessonId for new entries and the back link
            if (!lessonId) {
                alert('Error: A Lesson ID is required to add a new activity.');
                pageHeader.textContent = 'Error';
                form.style.display = 'none'; // Hide form if no lessonId
                return;
            }
            lessonIdInput.value = lessonId;
            backLink.href = `/Admin/Activities?lessonId=${lessonId}`;

            // Just wait for the dropdowns to finish loading
            await Promise.all(dropdownPromises);
        }
    }

    // Attach the submit event listener to the form
    form.addEventListener('submit', handleFormSubmit);

    // Start the page initialization process
    initializePage();
});