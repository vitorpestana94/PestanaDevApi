using PestanaDevApi.Dtos.Requests;
using PestanaDevApi.Extensions.Dtos.Requests;
using PestanaDevApi.Extensions.Models;
using PestanaDevApi.Models.Board;

namespace PestanaDevApi.Tests.Extensions
{
    public class UpdateBoardColumnRequestDtoExtensionsTests
    {
        private readonly BoardColumn _column = new() { Id = Guid.NewGuid(), BoardId = Guid.NewGuid(), Name = "Todo", Position = 2 };

        // The Dapper params are anonymous objects (internal to the API assembly), so they are read by reflection.
        private static object? GetValue(object param, string property) => param.GetType().GetProperty(property)!.GetValue(param);

        [Theory]
        [InlineData(99, 5)]
        [InlineData(-3, 1)]
        [InlineData(4, 4)]
        public void ToUpdate_ClampsNewPosition(int requestedPosition, int expectedPosition)
        {
            object update = new UpdateBoardColumnRequestDto { Position = requestedPosition }.ToUpdate(_column, columnsCount: 5);

            Assert.Equal(expectedPosition, GetValue(update, "NewPosition"));
            Assert.Equal(2, GetValue(update, "OldPosition"));
        }

        [Fact]
        public void ToUpdate_KeepsPositionWhenItIsNotSent()
        {
            object update = new UpdateBoardColumnRequestDto { Name = "Doing" }.ToUpdate(_column, columnsCount: 5);

            Assert.Equal(2, GetValue(update, "NewPosition"));
        }

        [Fact]
        public void WasDataNotUpdated_ReturnsTrueOnlyWhenNothingIsSent()
        {
            Assert.True(new UpdateBoardColumnRequestDto().WasDataNotUpdated());
            Assert.False(new UpdateBoardColumnRequestDto { Position = 1 }.WasDataNotUpdated());
        }

        [Fact]
        public void ColumnNameWasUpdated_IgnoresCase()
        {
            Assert.False(new UpdateBoardColumnRequestDto { Name = "TODO" }.ColumnNameWasUpdated("Todo"));
            Assert.True(new UpdateBoardColumnRequestDto { Name = "Doing" }.ColumnNameWasUpdated("Todo"));
        }
    }

    public class CreateBoardTaskRequestDtoExtensionsTests
    {
        [Fact]
        public void GetTaskOwnerId_UsesTheCreator_WhenOwnerIsNotSent()
        {
            Guid userId = Guid.NewGuid();
            Guid ownerId = Guid.NewGuid();

            Assert.Equal(userId, new CreateBoardTaskRequestDto().GetTaskOwnerId(userId));
            Assert.Equal(ownerId, new CreateBoardTaskRequestDto { TaskOwnerId = ownerId }.GetTaskOwnerId(userId));
        }
    }

    public class UpdateBoardRequestDtoExtensionsTests
    {
        [Fact]
        public void WasDataNotUpdated_ReturnsTrueOnlyWhenNothingIsSent()
        {
            Assert.True(new UpdateBoardRequestDto().WasDataNotUpdated());
            Assert.False(new UpdateBoardRequestDto { Description = "" }.WasDataNotUpdated());
        }
    }

    public class UpdateBoardTaskRequestDtoExtensionsTests
    {
        [Fact]
        public void WasDataNotUpdated_ReturnsTrueOnlyWhenNothingIsSent()
        {
            Assert.True(new UpdateBoardTaskRequestDto().WasDataNotUpdated());
            Assert.True(new UpdateBoardTaskRequestDto { ClearDeadline = false }.WasDataNotUpdated());
            Assert.False(new UpdateBoardTaskRequestDto { ColumnId = Guid.NewGuid() }.WasDataNotUpdated());
            Assert.False(new UpdateBoardTaskRequestDto { Deadline = DateTime.UtcNow }.WasDataNotUpdated());
            Assert.False(new UpdateBoardTaskRequestDto { ClearDeadline = true }.WasDataNotUpdated());
        }

        [Fact]
        public void IsMovingTask_ReturnsTrueWhenColumnIsSent()
        {
            Assert.True(new UpdateBoardTaskRequestDto { ColumnId = Guid.NewGuid() }.IsMovingTask());
            Assert.False(new UpdateBoardTaskRequestDto { TaskName = "x" }.IsMovingTask());
        }
    }

    public class ChangeUserDataRequestDtoExtensionsTests
    {
        [Fact]
        public void WasDataNotUpdated_ReturnsTrueOnlyWhenNothingIsSent()
        {
            Assert.True(new ChangeUserDataRequestDto().WasDataNotUpdated());
            Assert.False(new ChangeUserDataRequestDto { Name = "New name" }.WasDataNotUpdated());
        }

        [Fact]
        public void UserEmailWasUpdated_IgnoresCase()
        {
            Assert.False(new ChangeUserDataRequestDto { Email = "USER@email.com" }.UserEmailWasUpdated("user@email.com"));
            Assert.True(new ChangeUserDataRequestDto { Email = "new@email.com" }.UserEmailWasUpdated("user@email.com"));
        }
    }

    public class GetNasaAstronomyPicturesOfPeriodRequestExtensionsTests
    {
        [Theory]
        [InlineData("2024-01-01", "2024-01-07", true)]
        [InlineData("2024-01-07", "2024-01-01", false)]
        [InlineData("2024-01-01", null, false)]
        [InlineData("invalid", "2024-01-07", false)]
        public void AreBothDatesValids_ReturnsExpected(string? startDate, string? endDate, bool expected)
        {
            GetNasaAstronomyPicturesOfPeriodRequest request = new() { StartDate = startDate, EndDate = endDate };

            Assert.Equal(expected, request.AreBothDatesValids());
        }
    }

    public class BoardModelsExtensionsTests
    {
        [Fact]
        public void HasColumnName_IgnoresCase()
        {
            BoardColumn[] columns = [new() { Name = "Todo" }, new() { Name = "Done" }];

            Assert.True(columns.HasColumnName("TODO"));
            Assert.False(columns.HasColumnName("Doing"));
        }

        [Fact]
        public void IsBoardOwner_ComparesOwnerId()
        {
            Guid ownerId = Guid.NewGuid();
            Board board = new() { OwnerId = ownerId };

            Assert.True(board.IsBoardOwner(ownerId));
            Assert.False(board.IsBoardOwner(Guid.NewGuid()));
        }
    }
}
