# Git Worktree Workflow

This document outlines my preferred workflow for using git worktrees with GitHub Copilot assistance.

## Setup

- Main repository location: `/home/rich/git/core-rich`
- Worktree location: `/home/rich/git/core-rich/_worktree` (gitignored directory)

## Workflow Preferences

### General Guidelines

1. **Default behavior**: All work should be done in worktrees, not in the main repo area.
2. **Session awareness**: When starting a session, GitHub Copilot should check if I'm in the main repo area and prompt me to move to an appropriate worktree.

### Worktree Management

1. **Creating worktrees**:
   - Create worktrees in the `_worktree` directory
   - Name worktrees by feature/issue being worked on
   - Example: `git worktree add _worktree/feature-123 -b feature-123`

2. **Switching between worktrees**:
   - GitHub Copilot should offer to list existing worktrees and help switch between them
   - When switching, save current context/state to resume work easily

3. **Commit management**:
   - Help cherry-pick commits between worktrees when needed
   - Assist with rebasing worktree branches
   - Guide through merge conflict resolution
   - Track uncommitted changes when switching contexts

### Session Context

GitHub Copilot should maintain awareness of:

- Which worktree I'm currently working in
- What branch each worktree is on
- The purpose/focus of each worktree (based on branch name or my descriptions)

## Commands Reference

Common git worktree commands I use:

```bash
# Create a new worktree with a new branch
git worktree add _worktree/feature-name -b feature-name

# List all worktrees
git worktree list

# Remove a worktree
git worktree remove _worktree/feature-name

# Move between worktrees
cd _worktree/feature-name
```

## Worktree Status Tracking

GitHub Copilot should maintain a record of active worktrees in the WORKTREE_CONVERSATION_LOG.md file, updating it when:

- New worktrees are created
- Work context switches between worktrees
- Significant actions are performed in a worktree

This allows for persistent tracking across different VS Code sessions.

## Advanced Git Operations

### Context Switching

GitHub Copilot can assist with switching between worktree contexts by:

1. **Detecting uncommitted changes** before switching:
   - Offer to stash changes
   - Commit as work-in-progress
   - Show diff of current changes

2. **Preserving mental context**:
   - Save notes about current work in WORKTREE_CONVERSATION_LOG.md
   - Record TODO items when switching away
   - Summarize what was being worked on when returning to a worktree

3. **IDE state management**:
   - Track open files and editor positions
   - Remember terminal history and state
   - Manage running processes and background tasks

### Git State Management

GitHub Copilot can help manage git state across worktrees with:

1. **Branch synchronization**:
   - Track when branches need updating from main/master
   - Suggest when to pull latest changes
   - Help keep feature branches up to date

2. **Cherry-picking and rebasing**:
   - Identify commits to move between branches
   - Guide through interactive rebase operations
   - Explain complex git operations in simple terms

3. **History management**:
   - Compare branches across worktrees
   - Visualize branch relationships
   - Suggest branch cleanup when appropriate

### Merge Conflict Resolution

When merge conflicts inevitably occur, GitHub Copilot can:

1. **Detect and analyze conflicts**:
   - Show a summary of all conflicted files
   - Explain the nature of each conflict
   - Provide context about both conflicting changes

2. **Guide resolution strategies**:
   - Suggest appropriate resolution approaches
   - Explain the consequence of each choice
   - Help with semantic understanding of the conflicting code

3. **Automate when possible**:
   - Propose resolutions for simple conflicts
   - Identify patterns in conflict resolution
   - Help with repetitive conflict resolution tasks

4. **Post-resolution verification**:
   - Suggest tests to run after resolving conflicts
   - Help verify that the merged code works correctly
   - Ensure all conflicts were properly addressed

## Alternative Approaches

While this workflow uses the centralized `_worktree/` directory approach, there are several alternative patterns for git worktree management:

### 1. Sibling Directory Pattern

```text
projects/
├── myproject/           # main repo
├── myproject-feature-a/ # worktree
└── myproject-bugfix-b/  # worktree
```

**Pros:**

- Independent directories that survive main repo deletion
- Flat directory structure that's easy to navigate
- Works well with most IDEs and tools

**Cons:**

- Requires consistent naming discipline
- Context switching between different directory trees
- More complex for GitHub Copilot to track relationships

### 2. Home Directory Organization

```text
~/work/
├── repos/
│   └── myproject/       # main (bare or regular)
└── worktrees/
    ├── myproject-feature-a/
    └── myproject-feature-b/
```

**Pros:**

- Clean separation of concerns
- Works well across multiple projects
- Good for juggling many repos simultaneously

**Cons:**

- More disconnected sessions
- Requires longer, more specific naming conventions
- Higher cognitive overhead for context switching

### 3. Bare Repo + All Worktrees

```text
~/projects/
├── myproject.git/       # bare repo
├── myproject-main/      # main worktree
├── myproject-feature/   # feature worktree
```

**Pros:**

- No "special" main branch, all branches are treated equally
- Works well for project maintainers who need multiple long-lived branches
- Good separation of concerns

**Cons:**

- More complex initial setup
- Less intuitive for users new to git worktrees
- Requires more git knowledge

The centralized `_worktree/` approach was chosen for this workflow because it provides:

- Clear relationship between the main repo and its worktrees
- Simple navigation and discovery of worktrees
- Easy context management for GitHub Copilot
- Clean, contextual naming without redundancy
- Minimal cognitive overhead when switching contexts
